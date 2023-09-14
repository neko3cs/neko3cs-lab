import Head from 'next/head'
import Styles from './Home.module.css'
import Link from 'next/link';

export default function Home() {
  let title = "ともすた";

  return (
    <>
      {/* 最新バージョンだとlayout.tsxのMetadataに記載するのが一般的そう
      <Head>
        <title>ともすた</title>
      </Head>
      */}

      <h1 className={Styles.mytitle} style={{ backgroundColor: `#9f9` }}>{title}</h1>
      <p>学ぶ。をちゃんと</p>
      <div>
        <Link href="/about">
          About
        </Link>
      </div>
      {/*
      <style jsx>{`
        h1 {
          color: white;
        }
      `}</style>
      */}
    </>
  )
}