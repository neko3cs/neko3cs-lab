package cmd

import (
	"fmt"

	"github.com/spf13/cobra"
)

var (
	greetName  string
	greetCount int
)

var greetCmd = &cobra.Command{
	Use:   "greet",
	Short: "挨拶を表示する",
	RunE: func(cmd *cobra.Command, args []string) error {
		for i := 0; i < greetCount; i++ {
			fmt.Printf("Hello, %s!\n", greetName)
		}
		return nil
	},
}

func init() {
	rootCmd.AddCommand(greetCmd)

	greetCmd.Flags().StringVarP(&greetName, "name", "n", "world", "挨拶する相手の名前")
	greetCmd.Flags().IntVarP(&greetCount, "count", "c", 1, "繰り返す回数")
}
