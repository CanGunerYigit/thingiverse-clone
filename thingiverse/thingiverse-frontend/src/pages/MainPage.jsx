import { useState, useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";
import SortBar from "../components/Main/SortBar";
import ItemsGrid from "../components/Main/ItemsGrid";
import Pagination from "../components/Main/Pagination";

export default function MainPage() {
  const navigate = useNavigate();

  const [active, setActive] = useState("Popular");
  const [items, setItems] = useState([]);
  const [likedItems, setLikedItems] = useState([]);
  const [likedFetched, setLikedFetched] = useState(false);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [timeRange, setTimeRange] = useState("All Time");
  const dropdownRef = useRef(null);

  const itemsPerPage = 20;
  const token = localStorage.getItem("token");

  // Dropdown click dışına tıklayınca kapanması (SortBar içinde de benzer vardı, eğer SortBar kendi dropdownunu tutacaksa bunu oraya taşıyabilirsin)
  useEffect(() => {
    function handleClickOutside(event) {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        // setShowDropdown(false); // ShowDropdown artık SortBar'da yönetiliyor
      }
    }
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  // Kullanıcının beğenilerini çek
  useEffect(() => {
    let mounted = true;
    async function fetchLikedItems() {
      if (!token) {
        if (mounted) {
          setLikedItems([]);
          setLikedFetched(true);
        }
        return;
      }
      try {
        const res = await fetch("https://localhost:7267/api/Like/userlikes", {
          headers: { Authorization: `Bearer ${token}` },
        });
        if (!res.ok) throw new Error("Beğenilenler yüklenemedi.");
        const data = await res.json();
        if (!mounted) return;

        let ids = [];
        if (Array.isArray(data)) {
          ids = data
            .map((d) => {
              if (typeof d === "number") return d;
              if (d == null) return null;
              if (d.Item && (d.Item.Id ?? d.Item.id)) return d.Item.Id ?? d.Item.id;
              if (d.Id ?? d.id) return d.Id ?? d.id;
              if (typeof d === "object") {
                if (d.item && (d.item.id ?? d.item.Id)) return d.item.id ?? d.item.Id;
              }
              return null;
            })
            .filter((x) => x !== null);
        }
        setLikedItems(ids);
      } catch (error) {
        console.error("Beğenilenler yüklenemedi:", error);
        setLikedItems([]);
      } finally {
        if (mounted) setLikedFetched(true);
      }
    }
    fetchLikedItems();
    return () => {
      mounted = false;
    };
  }, [token]);

  // Aktif filtreye göre itemları çek
  useEffect(() => {
    if (!likedFetched) return;

    setLoading(true);

    let endpoint = "";
    let transformFn = (data) => data;

    const timeRangeEndpointMap = {
      "All Time": "alltime",
      "10 Months": "10months",
      "3 Years": "3years",
    };

    if (active === "Popular") {
      endpoint = `https://localhost:7267/api/ThingiVerse/popular/${timeRangeEndpointMap[timeRange]}`;
      transformFn = (data) =>
        data
          .map((item) => ({
            ...item,
            likedByCurrentUser: likedItems.includes(item.id),
            makes: item.makes || 0,
          }))
          .sort((a, b) => b.likes - a.likes);
    } else if (active === "Newest") {
      endpoint = "https://localhost:7267/api/Newest";
      transformFn = (data) =>
        data.map((item) => ({
          ...item,
          likedByCurrentUser: likedItems.includes(item.id),
          makes: item.makes || 0,
        }));
    } else if (active === "MostMakes") {
      endpoint = "https://localhost:7267/api/Makes/most-makes";
      transformFn = (data) =>
        data
          .map((apiItem) => ({
            ...apiItem.item,
            makes: apiItem.makeCount,
            likedByCurrentUser: likedItems.includes(apiItem.item.id),
          }))
          .sort((a, b) => b.makes - a.makes);
    }

    fetch(endpoint)
      .then((res) => {
        if (!res.ok) throw new Error("Veri yükleme hatası");
        return res.json();
      })
      .then((data) => {
        const updatedItems = transformFn(data);
        setItems(updatedItems);
        setCurrentPage(1);
        setLoading(false);
      })
      .catch((err) => {
        console.error("Veri yükleme hatası:", err);
        setLoading(false);
      });
  }, [active, timeRange, likedFetched, likedItems]);

  // likedItems güncellenince itemların likedByCurrentUser alanını güncelle
  useEffect(() => {
    if (!likedFetched || items.length === 0) return;
    setItems((prev) =>
      prev.map((item) => ({
        ...item,
        likedByCurrentUser: likedItems.includes(item.id),
      }))
    );
  }, [likedItems, likedFetched]);

  // Like toggle işlemi
  async function toggleLike(itemId) {
    if (!token) {
      alert("Lütfen giriş yapınız.");
      return;
    }

    // Optimistik UI güncellemesi
    setItems((prevItems) =>
      prevItems.map((item) =>
        item.id === itemId
          ? {
              ...item,
              likedByCurrentUser: !item.likedByCurrentUser,
              likes: item.likedByCurrentUser ? Math.max(0, item.likes - 1) : item.likes + 1,
            }
          : item
      )
    );

    setLikedItems((prev) =>
      prev.includes(itemId) ? prev.filter((id) => id !== itemId) : [...prev, itemId]
    );

    try {
      const res = await fetch(`https://localhost:7267/api/Like/toggle/${itemId}`, {
        method: "POST",
        headers: { Authorization: `Bearer ${token}` },
      });

      if (!res.ok) throw new Error("Like işlemi başarısız.");

      const data = await res.json();
      if (typeof data.liked === "boolean") {
        setLikedItems((prev) =>
          data.liked ? Array.from(new Set([...prev, itemId])) : prev.filter((id) => id !== itemId)
        );
        setItems((prevItems) =>
          prevItems.map((item) =>
            item.id === itemId ? { ...item, likedByCurrentUser: data.liked } : item
          )
        );
      }
    } catch (error) {
      console.error(error);
      // Geri al
      setItems((prevItems) =>
        prevItems.map((item) =>
          item.id === itemId
            ? {
                ...item,
                likedByCurrentUser: !item.likedByCurrentUser,
                likes: item.likedByCurrentUser ? item.likes + 1 : Math.max(0, item.likes - 1),
              }
            : item
        )
      );
      setLikedItems((prev) =>
        prev.includes(itemId) ? prev.filter((id) => id !== itemId) : [...prev, itemId]
      );
    }
  }

  const paginatedItems = items.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  const totalPages = Math.ceil(items.length / itemsPerPage);

  return (
    <div className="flex flex-col">
      <SortBar active={active} setActive={setActive} timeRange={timeRange} setTimeRange={setTimeRange} />
      <ItemsGrid
        items={paginatedItems}
        loading={loading}
        active={active}
        toggleLike={toggleLike}
        navigate={navigate}
      />
      <Pagination currentPage={currentPage} totalPages={totalPages} setCurrentPage={setCurrentPage} />
    </div>
  );
}
