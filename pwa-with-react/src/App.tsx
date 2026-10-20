import { useEffect, useState } from 'react'
import './App.css'

interface InstallPromptEvent extends Event {
  prompt: () => Promise<{ outcome: 'accepted' | 'dismissed' }>
}

interface InstalledRelatedApp {
  platform: string
  id?: string
}

interface NavigatorWithInstalledRelatedApps extends Navigator {
  getInstalledRelatedApps?: () => Promise<InstalledRelatedApp[]>
}

type InstallState = 'installed' | 'not-installed' | 'unknown'

const storageKey = 'pwa-lab-card-count'

function isStandalone() {
  return window.matchMedia('(display-mode: standalone)').matches ||
    ('standalone' in navigator && navigator.standalone === true)
}

function App() {
  const [installPrompt, setInstallPrompt] = useState<InstallPromptEvent | null>(null)
  const [standalone, setStandalone] = useState(isStandalone)
  const [installState, setInstallState] = useState<InstallState>(isStandalone() ? 'installed' : 'unknown')
  const [online, setOnline] = useState(navigator.onLine)
  const [controlled, setControlled] = useState(Boolean(navigator.serviceWorker?.controller))
  const [installMessage, setInstallMessage] = useState('')
  const [cardCount, setCardCount] = useState(() => {
    const saved = Number(window.localStorage.getItem(storageKey))
    return Number.isFinite(saved) && saved >= 0 ? saved : 0
  })

  useEffect(() => {
    let active = true
    const checkInstallation = async () => {
      if (isStandalone()) {
        setInstallState('installed')
        return
      }
      const getInstalledRelatedApps = (navigator as NavigatorWithInstalledRelatedApps).getInstalledRelatedApps
      if (!getInstalledRelatedApps) {
        setInstallState('unknown')
        return
      }
      try {
        const apps = await getInstalledRelatedApps.call(navigator)
        if (active) {
          setInstallState(apps.some((app) => app.platform === 'webapp' && app.id === `${location.origin}/`)
            ? 'installed' : 'unknown')
        }
      } catch {
        if (active) setInstallState('unknown')
      }
    }
    void checkInstallation()
    const onBeforeInstallPrompt = (event: Event) => {
      event.preventDefault()
      setInstallPrompt(event as InstallPromptEvent)
      setInstallState('not-installed')
      setInstallMessage('インストールの準備ができました。')
    }
    const onAppInstalled = () => {
      setInstallPrompt(null)
      setInstallState('installed')
      setInstallMessage('インストールされました。アプリ一覧から起動してみてください。')
    }
    const onOnline = () => setOnline(true)
    const onOffline = () => setOnline(false)
    const onControllerChange = () => setControlled(Boolean(navigator.serviceWorker?.controller))
    const displayMode = window.matchMedia('(display-mode: standalone)')
    const onDisplayModeChange = () => {
      setStandalone(isStandalone())
      void checkInstallation()
    }

    window.addEventListener('beforeinstallprompt', onBeforeInstallPrompt)
    window.addEventListener('appinstalled', onAppInstalled)
    window.addEventListener('online', onOnline)
    window.addEventListener('offline', onOffline)
    window.addEventListener('focus', checkInstallation)
    navigator.serviceWorker?.addEventListener('controllerchange', onControllerChange)
    displayMode.addEventListener('change', onDisplayModeChange)

    return () => {
      active = false
      window.removeEventListener('beforeinstallprompt', onBeforeInstallPrompt)
      window.removeEventListener('appinstalled', onAppInstalled)
      window.removeEventListener('online', onOnline)
      window.removeEventListener('offline', onOffline)
      window.removeEventListener('focus', checkInstallation)
      navigator.serviceWorker?.removeEventListener('controllerchange', onControllerChange)
      displayMode.removeEventListener('change', onDisplayModeChange)
    }
  }, [])

  async function install() {
    if (!installPrompt) return
    const prompt = installPrompt
    setInstallPrompt(null)
    try {
      const { outcome } = await prompt.prompt()
      setInstallMessage(outcome === 'accepted'
        ? 'インストールを承認しました。完了後にアプリ一覧を確認してください。'
        : '今回はキャンセルされました。ブラウザーのメニューからも追加できます。')
    } catch {
      setInstallMessage('インストール画面を開けませんでした。ブラウザーのメニューを確認してください。')
    }
  }

  function addCard() {
    setCardCount((count) => {
      const next = count + 1
      window.localStorage.setItem(storageKey, String(next))
      return next
    })
  }

  function resetCards() {
    window.localStorage.removeItem(storageKey)
    setCardCount(0)
  }

  return (
    <main className="page">
      <header className="hero">
        <div className="brand"><span className="brand-mark">P</span> PWA LAB</div>
        <div className="hero-content">
          <span className="eyebrow">INSTALL · OFFLINE · STANDALONE</span>
          <h1>ブラウザーから、<br /><span>アプリへ。</span></h1>
          <p>この小さな React アプリで、PWA のインストールとオフライン動作を実際に確かめられます。</p>
        </div>
        <div className="hero-orb" aria-hidden="true"><span>P</span></div>
      </header>

      <section className="panel install-panel" aria-labelledby="install-heading">
        <div className="section-number">01 / INSTALL</div>
        <h2 id="install-heading">ホーム画面に追加</h2>
        <p>対応するブラウザーでインストール可能になると、下のボタンが使えるようになります。</p>
        <button className="primary-button" type="button" onClick={install} disabled={!installPrompt || installState === 'installed'}>
          {standalone ? 'アプリとして起動中' : installState === 'installed' ? 'アプリインストール済み' : installPrompt ? 'アプリをインストール' : installState === 'unknown' ? 'インストール状態を確認できません' : 'インストール待機中'}
          <span aria-hidden="true">↗</span>
        </button>
        <p className="hint" role="status">{installMessage || (standalone
          ? '独立したアプリ画面で起動しています。'
          : installState === 'installed' ? 'インストール済みです。アプリ一覧から起動できます。'
            : installState === 'unknown' ? 'このブラウザーではインストール状態を確認できません。ブラウザーのメニューも確認してください。'
              : 'ボタンが有効にならない場合は、ブラウザーのメニューから「インストール」または「ホーム画面に追加」を選んでください。')}</p>
      </section>

      <section className="panel" aria-labelledby="status-heading">
        <div className="section-number">02 / STATUS</div>
        <h2 id="status-heading">いまの状態</h2>
        <div className="status-grid">
          <div className="status-item"><span>表示モード</span><strong>{standalone ? 'アプリ画面' : 'ブラウザー'}</strong></div>
          <div className="status-item"><span>ネットワーク</span><strong>{online ? 'オンライン' : 'オフライン'}</strong></div>
          <div className="status-item"><span>Service Worker</span><strong>{controlled ? '有効' : '準備中'}</strong></div>
        </div>
        <p className="hint">Service Worker は初回表示後に有効化されます。「準備中」の場合は一度再読み込みしてください。{import.meta.env.DEV && 'オフラインでの再読み込みは pnpm build → pnpm preview で確認してください。'}</p>
      </section>

      <section className="panel" aria-labelledby="offline-heading">
        <div className="section-number">03 / OFFLINE</div>
        <h2 id="offline-heading">オフラインで試す</h2>
        <p>カードを追加してからネットワークを切り、ページを再読み込みしてください。画面とカードの数が残れば成功です。</p>
        <div className="card-demo"><div className="demo-icon" aria-hidden="true">✦</div><div><small>保存したカード</small><strong>{cardCount} 枚</strong></div></div>
        <button className="secondary-button" type="button" onClick={addCard}>カードを追加 <span aria-hidden="true">＋</span></button>
        <button className="secondary-button reset-button" type="button" onClick={resetCards}>カードをリセット <span aria-hidden="true">↺</span></button>
        <p className="hint">リセットすると localStorage の「{storageKey}」キーを削除します。</p>
      </section>
      <footer>React + Vite + Service Worker <span>·</span> PWA LAB</footer>
    </main>
  )
}

export default App
