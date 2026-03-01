# Plan: プロジェクトの全体的なリファクタリングとクリーンアップ

## フェーズ 1: Prettierによるコードフォーマットの統一 [checkpoint: 61bc14a]

- [x] Task: Prettierの構成確認と調整 (94df017)
  - [x] `.prettierrc` または `package.json` 内の設定を確認し、必要に応じてプロジェクトのスタイルガイドに合わせる。
  - [x] `.prettierignore` を確認し、ビルド成果物などが除外されているか確認する。
- [x] Task: 全ファイルへのフォーマット適用 (e7be0dc)
  - [x] プロジェクト全体のファイルを対象に Prettier を実行し、コードスタイルを統一する。- [x] Task: フォーマット済みの検証
    - [x] `pnpm format` (チェックモード) を実行し、すべてのファイルが正しくフォーマットされていることを確認する。
- [x] Task: Conductor - User Manual Verification 'Prettierによるコードフォーマットの統一' (Protocol in workflow.md)

## フェーズ 2: 静的解析エラーおよび警告の解消

- [ ] Task: 現状の問題点の洗い出し
  - [ ] `pnpm lint` および `pnpm typecheck` を実行し、現在発生しているエラー・警告の一覧を取得する。
- [ ] Task: Lintエラー・警告の修正
  - [ ] ESLint で報告されるすべての問題を修正する。可能なものは `--fix` で自動修正し、残りは手動で修正する。
- [ ] Task: 型エラー・警告の修正
  - [ ] TypeScript の型チェックで報告されるすべての問題を修正する。特に `any` の使用や、不適切な型推論を改善する。
- [ ] Task: 最終的な静的解析パスの確認
  - [ ] 再度 `pnpm lint` と `pnpm typecheck` を実行し、エラーがゼロであることを確認する。
- [ ] Task: Conductor - User Manual Verification '静的解析エラーおよび警告の解消' (Protocol in workflow.md)

## フェーズ 3: 最終検証と機能整合性の確認

- [ ] Task: 自動テストスイートの実行
  - [ ] すべてのユニットテスト (`pnpm test`) と E2E テスト (`npx playwright test`) を実行し、リファクタリングによる破損がないことを確認する。
- [ ] Task: Conductor - User Manual Verification '最終検証と機能整合性の確認' (Protocol in workflow.md)
