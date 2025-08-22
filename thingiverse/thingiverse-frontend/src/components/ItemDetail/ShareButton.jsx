import React, { useState } from "react";
import { Copy } from "lucide-react"; 

export default function ShareButton() {
  const [copied, setCopied] = useState(false);

  const copyUrl = () => {
    const url = window.location.href; // sayfa urli
    navigator.clipboard.writeText(url).then(() => {
      setCopied(true);
      setTimeout(() => setCopied(false), 2000);
    });
  };

  return (
    <button
      onClick={copyUrl}
      className="h-[40px] flex items-center gap-2 px-4 py-2 rounded-lg bg-yellow-400 text-white hover:bg-yellow-500 transition "
    >
      <Copy size={18} />
      {copied ? "Kopyalandı!" : "Copy the link"}
    </button>
  );
}
