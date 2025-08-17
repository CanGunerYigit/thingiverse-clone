import React, { useState } from "react";
import LoginHeader from "../components/Login/LoginHeader";
import LoginWelcome from "../components/Login/LoginWelcome";
import LoginForm from "../components/Login/LoginForm";

export default function LoginPage() {
  const [form, setForm] = useState({ username: "", password: "" });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleChange = (e) => {
    setForm((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };

  const handleLogin = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const res = await fetch("https://localhost:7267/api/Account/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(form),
      });

      const text = await res.text();
      console.log("Raw login response:", text);

      if (!res.ok) {
        let data;
        try {
          data = JSON.parse(text);
        } catch {
          data = {};
        }
        setError(data.message || "Giriş başarısız.");
        setLoading(false);
        return;
      }

      const data = JSON.parse(text);
      console.log("Parsed login data:", data);

      // Token ve userId'yi localStorage'a kaydet
      localStorage.setItem("token", data.token);
      // Login response yapısına göre userId alıyoruz
      localStorage.setItem("userId", data.user?.id || data.id);
      localStorage.setItem("user", JSON.stringify(data));

      // Login sonrası ana sayfaya yönlendir
      window.location.href = "/";
    } catch (err) {
      console.error(err);
      setError("Sunucuya bağlanırken bir hata oluştu.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-white">
      <LoginHeader />
      <LoginWelcome />
      <LoginForm
        form={form}
        onChange={handleChange}
        onSubmit={handleLogin}
        loading={loading}
        error={error}
      />
    </div>
  );
}
