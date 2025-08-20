import React from "react";

export default function InputField({
  id,
  label,
  type = "text",
  value,
  onChange,
  required = false,
  placeholder,
  rows,
  accept, // file input için
}) {
  const commonProps = {
    id,
    onChange,
    required,
    placeholder,
    className:
      "w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500",
  };

  // file input için value prop kaldırıyoruz
  if (type !== "file") {
    commonProps.value = value;
  }

  return (
    <div>
      <label
        htmlFor={id}
        className="block text-sm font-medium text-gray-700 mb-1"
      >
        {label}
        {required && "*"}
      </label>

      {type === "textarea" ? (
        <textarea {...commonProps} rows={rows || 4} />
      ) : type === "file" ? (
        <input {...commonProps} type="file" accept={accept} />
      ) : (
        <input {...commonProps} type={type} />
      )}
    </div>
  );
}
