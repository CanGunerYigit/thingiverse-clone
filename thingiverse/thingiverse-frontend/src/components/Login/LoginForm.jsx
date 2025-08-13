import React from "react";
import { Link } from "react-router-dom";
import laptop from "../../assets/Macbook7e824d51bda967158a331fcefaaa1d6e.svg";
import laptopImage from "../../assets/thingiverseAd753e4b07fa65cfda61aa911519ab8314.png";

export default function LoginForm({ form, onChange, onSubmit, loading, error }) {
  return (
    <div className="flex flex-col lg:flex-row justify-center items-center lg:items-start gap-8 lg:gap-12 px-4 sm:px-6 pb-8 sm:pb-16 max-w-7xl mx-auto">
      <div className="hidden md:flex justify-center items-center w-full lg:w-auto">
        <div className="relative w-full max-w-[500px] lg:w-[600px] xl:w-[806px] h-auto aspect-[806/593]">
          <img
            src={laptop}
            alt="Laptop Frame"
            className="w-full h-full object-contain relative z-10"
          />
          <img
            src={laptopImage}
            alt="Laptop Screen"
            className="absolute top-[20%] left-[13.5%] w-[73%] h-[60%] object-cover rounded-sm z-0"
          />
        </div>
      </div>

      <div className="w-full max-w-md bg-white rounded-xl shadow-lg p-4 sm:p-6 lg:mt-16">
        <h2 className="text-xl font-semibold mb-4 text-center">Sign In</h2>

        <button
          type="button"
          className="w-full flex items-center justify-center gap-2 bg-white border border-gray-300 text-sm py-2 px-4 rounded-md shadow-sm hover:bg-gray-100 mb-3"
        >
          <img
            src="https://www.svgrepo.com/show/475656/google-color.svg"
            alt="Google"
            className="w-5 h-5"
          />
          SIGN UP WITH GOOGLE
        </button>

        <p className="text-center text-sm text-gray-500 my-3">or sign up with email</p>

        <form className="space-y-3" onSubmit={onSubmit}>
          <input
            type="text"
            name="username"
            placeholder="Username"
            value={form.username}
            onChange={onChange}
            required
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring focus:ring-blue-200"
          />
          <input
            type="password"
            name="password"
            placeholder="Password"
            value={form.password}
            onChange={onChange}
            required
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring focus:ring-blue-200"
          />
          <button
            type="submit"
            disabled={loading}
            className="w-full bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 rounded-md disabled:opacity-70"
          >
            {loading ? "Signing in..." : "CONTINUE"}
          </button>
        </form>

        {error && <p className="text-red-500 text-sm mt-3 text-center">{error}</p>}

        <p className="text-sm text-center mt-4 text-gray-600">
          Don't have a MakerBot Account?{" "}
          <Link to="/signup" className="text-blue-600 font-semibold hover:underline">
            SIGN UP
          </Link>
        </p>
      </div>
    </div>
  );
}
