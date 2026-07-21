package cmd

import (
	"github.com/spf13/cobra"
)

// rootCmd はサブコマンドなしで呼ばれた時のベースコマンド
var rootCmd = &cobra.Command{
	Use:   "go_cobra_cli",
	Short: "Go Cobra CLI",
	Long:  "Go Cobra CLI は、Go の Cobra ライブラリを使用した CLI ツールのサンプルです。",
}

// Execute は main.go から呼ばれるエントリーポイント
func Execute() error {
	return rootCmd.Execute()
}

func init() {
	// 全サブコマンドに継承させたいグローバルフラグはここに書く
	// 例: rootCmd.PersistentFlags().BoolP("verbose", "v", false, "詳細出力")
}
