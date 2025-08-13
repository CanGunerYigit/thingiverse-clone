import React, { useState } from "react";
import RegisterHeader from "../components/Register/RegisterHeader";
import RegisterWelcome from "../components/Register/RegisterWelcome";
import RegisterForm from "../components/Register/RegisterForm";
import { useNavigate } from "react-router-dom";

export default function RegisterPage() {
  const [email, setEmail] = useState("");
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const res = await fetch("https://localhost:7267/api/Account/register", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          username,
          email,
          password,
        }),
      });

      const errData = await res.json();

      if (!res.ok) {
        if (res.status === 404) {
          setError("Hata oluştu");
        } else if (
          res.status === 400 &&
          errData.message?.toLowerCase().includes("username")
        ) {
          setError("Kullanıcı adı alınmış");
        } else if (
          res.status === 400 &&
          errData.message?.toLowerCase().includes("password")
        ) {
          setError("Şifre en az bir büyük harf, rakam ve karakter içermeli");
        } else {
          setError(errData.message || "Kayıt işlemi başarısız.");
        }
        setLoading(false);
        return;
      }

      localStorage.setItem("token", errData.token);
      localStorage.setItem("user", JSON.stringify(errData));
      navigate("/");
    } catch (err) {
      setError("Sunucuya bağlanırken bir hata oluştu.");
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-white">
      <RegisterHeader />
      <RegisterWelcome />
      <RegisterForm
        email={email}
        setEmail={setEmail}
        username={username}
        setUsername={setUsername}
        password={password}
        setPassword={setPassword}
        loading={loading}
        error={error}
        onSubmit={handleRegister}
      />
    </div>
  );
}
