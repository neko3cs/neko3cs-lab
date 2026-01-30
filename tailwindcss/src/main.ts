console.log('Tailwind 開発スタート！ 🎨');

const btn = document.querySelector<HTMLButtonElement>('#alertBtn');

btn?.addEventListener('click', () => {
  alert('ナイス！その調子で頑張りましょう！🔥');
});
