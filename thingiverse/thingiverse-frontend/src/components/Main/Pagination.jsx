export default function Pagination({ currentPage, totalPages, setCurrentPage }) {
  const pageGroupSize = 5;
  const currentGroup = Math.floor((currentPage - 1) / pageGroupSize);
  const groupStart = currentGroup * pageGroupSize + 1;
  const groupEnd = Math.min(groupStart + pageGroupSize - 1, totalPages);

  return (
    <div className="flex flex-wrap justify-center items-center gap-2 py-6 text-gray-700 text-sm font-normal">
      <button
        className={`px-2 py-1 hover:text-gray-900 ${
          currentPage === 1 ? "text-gray-400 cursor-not-allowed hover:text-gray-400" : ""
        }`}
        onClick={() => currentPage > 1 && setCurrentPage(currentPage - 1)}
        aria-label="Previous page"
        disabled={currentPage === 1}
      >
        {"<"}
      </button>

      {Array.from({ length: groupEnd - groupStart + 1 }, (_, i) => groupStart + i).map((page) => (
        <button
          key={page}
          onClick={() => setCurrentPage(page)}
          className={`relative px-2 py-1 duration-200 ${
            page === currentPage ? "text-gray-900 font-semibold border-b-4 border-black" : "hover:text-gray-900 font-semibold text-sm"
          }`}
          aria-current={page === currentPage ? "page" : undefined}
        >
          {page}
        </button>
      ))}

      <button
        className={`px-2 py-1 hover:text-gray-900 ${
          currentPage === totalPages ? "text-gray-400 cursor-not-allowed hover:text-gray-400" : ""
        }`}
        onClick={() => currentPage < totalPages && setCurrentPage(currentPage + 1)}
        aria-label="Next page"
        disabled={currentPage === totalPages}
      >
        {">"}
      </button>
    </div>
  );
}
