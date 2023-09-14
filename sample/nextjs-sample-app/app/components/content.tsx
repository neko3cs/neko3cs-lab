import { ReactNode } from "react";

export default function Content(props: {
  children: ReactNode
}) {
  return (
    <>
      <div className="container">
        {props.children}
      </div>
      <hr />
      <footer>©︎ 2021 ともすた</footer>
    </>
  );
}