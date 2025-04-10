import { Button } from "antd";
import Image from "next/image";
import Link from "next/link";

type CatImage = {
  id: string;
  url: string;
  width: number;
  height: number;
};

export default async function About() {
  const res = await fetch("https://api.thecatapi.com/v1/images/search");
  const catImage: CatImage = (await res.json())[0];

  return (
    <div>
      <h1>About Us</h1>
      <Image src={catImage.url} alt="Cat" width={catImage.width} height={catImage.height} />
      <p>This is the about page of our application.</p>

      <Link href="/">
        <Button type="primary">Home</Button>
      </Link>
    </div>
  );
}