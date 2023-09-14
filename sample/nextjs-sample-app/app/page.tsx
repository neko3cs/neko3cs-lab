// import Head from 'next/head'
// import Styles from './Home.module.css'
import Link from 'next/link';
import Header from './components/header';
import Content from './components/content';

export default function Home() {
  let title = "ともすた";

  return (
    <Content>
      {/* 最新バージョンだとlayout.tsxのMetadataに記載するのが一般的そう
      <Head>
        <title>ともすた</title>
      </Head>
      */}

      < Header title={title} />
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
    </Content>
  )
}