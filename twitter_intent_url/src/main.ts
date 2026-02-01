const createXPostUrl = (
  text: string,
  options?: { hashtags?: string[]; url?: string; via?: string }
): string => {
  const baseUrl = "https://x.com/intent/post";
  const params = new URLSearchParams();

  params.append("text", text);
  if (options?.hashtags) {
    params.append("hashtags", options.hashtags.join(","));
  }
  if (options?.url) {
    params.append("url", options.url);
  }
  if (options?.via) {
    params.append("via", options.via);
  }

  return `${baseUrl}?${params.toString()}`;
};

const url = createXPostUrl(
  "アプリを開発しました！🚀 #個人開発 #TypeScript",
  {
    hashtags: [],
    url: "https://my-app.example.com",
    via: "YourAccountName"
  });

console.log(url);
// window.open(url, "_blank", "noreferrer"); // ブラウザならこう
