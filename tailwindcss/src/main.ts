console.log('Tailwind 開発スタート！ 🎨');

const alertBtn = document.querySelector<HTMLButtonElement>('#alertBtn');

alertBtn?.addEventListener('click', () => {
  alert('ナイス！その調子で頑張りましょう！🔥');
});


const colorBtn = document.querySelector<HTMLButtonElement>('#colorBtn');

colorBtn?.addEventListener('click', () => {
  // bodyの背景色を Tailwind クラスで切り替える
  document.body.classList.toggle('bg-gray-100');
  document.body.classList.toggle('bg-yellow-200');

  // ボタンの色もついでに変えてみる
  colorBtn.classList.toggle('bg-blue-500');
  colorBtn.classList.toggle('bg-purple-600');

  console.log('クラスを切り替えました！');
});
