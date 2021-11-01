const confirmTweet = (e) => {
  if (confirm('投稿します。よろしいですか？') === false) {
    e.stopPropagation();
  }
};

const onLoad = () => {
  const alertButton = document.querySelector('button#alertButton');

  if (alertButton) {
    console.log('ボタン見つけた！ダイアログを埋め込むね！');
    alertButton.addEventListener('click', confirmTweet, { capture: true });
  } else {
    console.log('ボタン見つからなかった！');
  }
};

window.addEventListener('load', onLoad);
