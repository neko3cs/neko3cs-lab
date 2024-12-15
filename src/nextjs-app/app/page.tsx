'use client';
import useSWR from 'swr';
// import Head from 'next/head'
import Link from 'next/link';
// import Styles from './Home.module.css'
import Header from './components/header';
import Content from './components/content';

const fetcher = (url: string) => fetch(url)
  .then(
    (res) => res.json(),
    (error) => console.log(error)
  );

export default function Home() {
  let title = "ともすた";
  const { data, error } = useSWR(
    "/api/message",
    fetcher
  );

  // FIXME: 404 Not Foundになってしまう
  if (error) return <div>failed to load</div>
  if (!data) return <div>loading...</div>

  return (
    <Content>
      {/* 最新バージョンだとlayout.tsxのMetadataに記載するのが一般的そう
      <Head>
        <title>ともすた</title>
      </Head>
      */}

      <Header title={title} />
      <p>{data.message}</p>
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