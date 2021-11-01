const showAlert = () => {
  const availableURL = "file:///C:/src/neko3cs-lab/sample-chrome-extensions/src/index.html";
  
  if (document.URL.match(availableURL)) {
    alert("この拡張機能はこのサイトで使えます。");
  } else {
    alert("この拡張機能はこのサイトで使えません。");
  }
}

chrome.action.onClicked.addListener((tab) => {
  chrome.scripting.executeScript({
    target: { tabId: tab.id },
    function: showAlert
  });
});
