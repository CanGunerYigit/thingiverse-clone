export default function ItemsGrid({ items, loading, active, toggleLike, navigate }) {
  function formatLikes(num) {
    return num >= 1000 ? Math.floor(num / 1000) + "K" : num.toString();
  }

  if (loading) return <p className="p-4">Loading items...</p>;
  if (!loading && items.length === 0) return <p className="p-4">No items to show.</p>;

  return (
    <div className="p-4 sm:p-6 grid  grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-4 sm:gap-6">
      {items.map((item) => (
        <div key={item.id} className="cursor-pointer">
          <img
            src={item.thumbnail}
            alt={item.name}
            className="w-full h-40 sm:h-48 object-cover rounded-lg hover:opacity-90 transition duration-150"
            onClick={() => navigate(`/item/${item.id}`)}
          />

          <div className="flex items-center justify-between mt-2">
            <h3
              className="font-medium text-sm truncate hover:text-blue-600"
              onClick={() => navigate(`/item/${item.id}`)}
            >
              {item.name}
            </h3>

            <div className="flex items-center gap-2 text-gray-500 text-sm">
              <span>{active === "MostMakes" ? formatLikes(item.makes || 0) : formatLikes(item.likes)}</span>

              <button
                onClick={(e) => {
                  e.stopPropagation();
                  toggleLike(item.id);
                }}
                className="focus:outline-none"
                aria-label={item.likedByCurrentUser ? "Unlike" : "Like"}
              >
                {item.likedByCurrentUser ? (
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    className="h-5 w-5 text-red-500"
                    fill="currentColor"
                    viewBox="0 0 24 24"
                    stroke="none"
                  >
                    <path d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 11.54L12 21.35z" />
                  </svg>
                ) : (
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    className="h-5 w-5 text-gray-400 hover:text-red-500 transition-colors"
                    fill="none"
                    viewBox="0 0 24 24"
                    stroke="currentColor"
                    strokeWidth={2}
                  >
                    <path strokeLinecap="round" strokeLinejoin="round" d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 11.54L12 21.35z" />
                  </svg>
                )}
              </button>
            </div>
          </div>

          <div className="flex items-center gap-2 mt-1">
            <img src={item.creatorAvatar} alt={item.creatorName} className="w-5 h-5 rounded-full" />
            <p className="text-gray-500 text-sm">{item.creatorName}</p>
          </div>
        </div>
      ))}
    </div>
  );
}
