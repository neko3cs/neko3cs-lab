package cmd

import (
	"fmt"

	"github.com/spf13/cobra"
)

var weatherCmd = &cobra.Command{
	Use:   "weather [city]",
	Short: "天気情報を取得する（サンプル）",
	Args:  cobra.ExactArgs(1),
	RunE: func(cmd *cobra.Command, args []string) error {
		city := args[0]
		fmt.Printf("%s の天気: 晴れ（サンプル固定値）\n", city)
		return nil
	},
}

func init() {
	rootCmd.AddCommand(weatherCmd)
}
