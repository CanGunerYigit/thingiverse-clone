import React from "react";
import stars from "../assets/starts.png";

export default function Hero() {
  return (
    <div
      className="text-white w-full h-[200px] sm:h-[280px] md:h-[350px] lg:h-[400px] bg-center bg-cover"
      style={{ 
        backgroundImage: `url(${stars})`,
      
        backgroundColor: '#1a365d' //bg image çalışmazsa
      }}
    >
<div className="w-full max-w-7xl mx-auto flex flex-col items-center justify-center h-full text-center px-4">
        <h1 className="text-3xl sm:text-4xl md:text-5xl lg:text-6xl font-bold">
          Welcome to Thingiverse
        </h1>
        <p className="text-base sm:text-lg md:text-xl mt-3 sm:mt-4 max-w-2xl">
          Digital Designs for Physical Objects
        </p>
      </div>
    </div>
  );
}