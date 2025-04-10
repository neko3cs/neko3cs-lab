import Link from "next/link";
import styles from "./page.module.css";
import { Button } from "antd";

export default function Home() {
  return (
    <div className={styles.page}>
      <main className={styles.main}>
        <h1>
          Hello World!
        </h1>

        <Link href="/about">
          <Button type="primary">About</Button>
        </Link>
      </main>
    </div>
  );
}
