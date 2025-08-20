import { useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";
export default function MakesTab({ makes, loading }) {
  const navigate = useNavigate();

  // normalize backend response (Id -> id, PascalCase -> camelCase)
  const normalizeMake = (m, idx) => ({
    id: m.id ?? m.Id ?? idx,
    name: m.name ?? m.Name,
    description: m.description ?? m.Description,
    thumbnail: m.thumbnail?.trim() ?? m.Thumbnail?.trim(),
    previewImage: m.previewImage?.trim() ?? m.PreviewImage?.trim(),
    createdAt: m.createdAt ?? m.CreatedAt,
    imagePaths: m.imagePaths ?? m.ImagePaths,
    itemId: m.itemId ?? m.ItemId,
    userId: m.userId ?? m.UserId,
    userName: m.userName ?? m.UserName,
  });

  const normalizedMakes = makes.map((m, i) => normalizeMake(m, i));

  const parseImagePaths = (imagePaths) => {
    if (!imagePaths) return [];
    if (Array.isArray(imagePaths)) return imagePaths;

    if (typeof imagePaths === "string") {
      try {
        const parsed = JSON.parse(imagePaths);
        if (Array.isArray(parsed)) return parsed;
      } catch {
        return [imagePaths.trim()];
      }
      return [imagePaths.trim()];
    }
    return [];
  };

  return (
    <div className="p-4 bg-white rounded-lg">
      <h2 className="text-xl font-semibold mb-3">Makes</h2>
      {loading ? (
        <p>Makes yükleniyor...</p>
      ) : normalizedMakes.length === 0 ? (
        <p>Henüz make eklenmemiş.</p>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          {normalizedMakes.map((make) => (
            <Link
              key={make.id}   
              to={`/makes/${make.id}`}
              className="block border rounded-lg overflow-hidden shadow-sm hover:shadow-md transition-shadow"
            >
              <div className="p-4">
                {/* Kullanıcı bilgisi */}
                <div className="flex items-center gap-3 mb-3">
                  <button
                    type="button"
                    onClick={(e) => {
                      e.preventDefault();
                      e.stopPropagation();
                      navigate(`/user/${make.userId}`);
                    }}
                    className="font-medium text-blue-600 hover:underline"
                  >
                    {make.userName}
                  </button>
                  <span className="text-gray-500 text-sm">
                    {new Date(make.createdAt).toLocaleDateString("tr-TR")}
                  </span>
                </div>

                {/* Açıklama */}
                {make.description && (
                  <p className="text-gray-700 mb-3">{make.description}</p>
                )}

                {/* Görseller */}
                <div className="space-y-2">
                  {/* Thumbnail */}
                  {make.thumbnail && (
                    <div className="relative h-48 bg-gray-100 rounded overflow-hidden">
                      <img
                        src={
                          make.thumbnail.startsWith("http")
                            ? make.thumbnail
                            : `https://localhost:7267${make.thumbnail}`
                        }
                        alt={`${make.userName}'s make thumbnail`}
                        className="w-full h-full object-contain"
                        onError={(e) => {
                          e.target.src =
                            "https://via.placeholder.com/300x200?text=Thumbnail+Not+Found";
                        }}
                      />
                    </div>
                  )}

                  {/* ImagePaths */}
                  {parseImagePaths(make.imagePaths).length > 0 && (
                    <div className="grid grid-cols-3 gap-2 mt-2">
                      {parseImagePaths(make.imagePaths).map((path, index) => (
                        <div
                          key={`${make.id}-${index}`}   // benzersiz key
                          className="relative h-24 bg-gray-100 rounded overflow-hidden"
                        >
                          <img
                            src={
                              path.startsWith("http")
                                ? path.trim()
                                : `https://localhost:7267${path.trim()}`
                            }
                            alt={`Make image ${index + 1}`}
                            className="w-full h-full object-cover"
                            onError={(e) => {
                              e.target.src =
                                "https://via.placeholder.com/150?text=Image+Not+Found";
                            }}
                          />
                        </div>
                      ))}
                    </div>
                  )}
                </div>
              </div>
            </Link>
          ))}
        </div>
      )}
    </div>
  );
}
