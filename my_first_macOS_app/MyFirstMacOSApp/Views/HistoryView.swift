//
//  HistoryView.swift
//  MyFirstMacOSApp
//
//  Created by neko3cs on 2026/02/14.
//

import SwiftUI
import SwiftData

struct HistoryView: View {
    @Environment(\.modelContext) private var modelContext
    var history: [HistoryItem]
    
    var body: some View {
        List {
            ForEach(history) { item in
                historyRow(for: item)
            }
            // リストの削除操作（スワイプ等）を有効化
            .onDelete(perform: deleteItems)
        }
        .navigationTitle("履歴")
        .toolbar {
            ToolbarItem(placement: .primaryAction) {
                Button(role: .destructive, action: resetHistory) {
                    Label("すべて削除", systemImage: "trash")
                }
                .disabled(history.isEmpty)
            }
        }
        .overlay {
            if history.isEmpty {
                emptyStateView
            }
        }
    }
    
    private func historyRow(for item: HistoryItem) -> some View {
        HStack {
            Image(systemName: "calendar")
                .foregroundStyle(.blue)
            Text(item.timestamp.formatted(date: .numeric, time: .standard))
        }
        // 追加時（横からスライド）と削除時（フェード）で異なるアニメーションを適用
        .transition(.asymmetric(insertion: .move(edge: .leading).combined(with: .opacity), removal: .opacity))
    }
    
    private var emptyStateView: some View {
        ContentUnavailableView(
            "履歴なし",
            systemImage: "clock",
            description: Text("カウントするとここに表示されます")
        )
    }
    
    private func resetHistory() {
        withAnimation {
            do {
                try modelContext.delete(model: HistoryItem.self)
                try modelContext.save()
            } catch {
                print("Failed to clear history: \(error)")
            }
        }
    }
    
    private func deleteItems(offsets: IndexSet) {
        withAnimation {
            for index in offsets {
                modelContext.delete(history[index])
            }
            do {
                try modelContext.save()
            } catch {
                print("Failed to delete items: \(error)")
            }
        }
    }
}
