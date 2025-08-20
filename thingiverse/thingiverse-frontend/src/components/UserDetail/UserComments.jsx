import React from "react";

export default function UserComments({ comments }) {
  if (!comments || comments.length === 0) {
    return (
      <div className="bg-white rounded-lg shadow-md p-6 mb-8 text-center py-8">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          className="h-12 w-12 mx-auto text-gray-400"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            strokeWidth={2}
            d="M8 12h.01M12 12h.01M16 12h.01M21 12c0 4.418-4.03 8-9 8a9.863 9.863 0 01-4.255-.949L3 20l1.395-3.72C3.512 15.042 3 13.574 3 12c0-4.418 4.03-8 9-8s9 3.582 9 8z"
          />
        </svg>
        <p className="mt-2 text-gray-500">This user hasn't made any comments yet</p>
      </div>
    );
  }

  return (
    <div className="bg-white rounded-lg shadow-md p-6 mb-8">
      <h2 className="text-2xl font-bold text-gray-800 mb-6 pb-2 border-b border-gray-200">
        User Comments
        <span className="ml-2 text-sm font-normal text-gray-500">({comments.length})</span>
      </h2>

      <div className="space-y-4">
        {comments.map((comment) => (
          <div
            key={comment.Id}
            className="border border-gray-200 rounded-lg p-4 hover:bg-gray-50 transition-colors"
          >
            <div className="flex justify-between items-start">
              <div>
                {/* Always go to our ItemDetail page */}
                <a
                  href={`/item/${comment.ItemId}`}
                  className="text-blue-600 hover:text-blue-800 font-medium"
                >
                  {comment.Name}
                </a>

                {/* Comment message */}
                <p className="text-gray-800 mt-2">{comment.Message}</p>
              </div>

              {/* Comment date */}
              <span className="text-xs text-gray-500 whitespace-nowrap">
                {new Date(comment.CreatedAt).toLocaleString("tr-TR")}
              </span>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
