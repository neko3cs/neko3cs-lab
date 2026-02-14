//
//  ContentView.swift
//  MyFirstMacOSApp
//
//  Created by neko3cs on 2026/02/14.
//

import SwiftUI
import SwiftData

struct ContentView: View {
    @Query(sort: \HistoryItem.timestamp, order: .reverse) private var history: [HistoryItem]
    @Environment(\.modelContext) private var modelContext
    @State private var selectedItem: SidebarItem? = .counter
    
    var body: some View {
        NavigationSplitView {
            SidebarListView(selection: $selectedItem)
        } detail: {
            detailView
        }
    }
    
    @ViewBuilder
    private var detailView: some View {
        if let item = selectedItem {
            switch item {
            case .counter:
                CounterView(
                    count: history.count,
                    onAdd: addRecord,
                    onReset: resetRecord
                )
            case .history:
                HistoryView(history: history)
            }
        } else {
            ContentUnavailableView("選択してください", systemImage: "sidebar.left")
        }
    }

    private func addRecord() {
        withAnimation(.spring(duration: 0.4, bounce: 0.3)) {
            let newItem = HistoryItem(timestamp: Date())
            modelContext.insert(newItem)
        }
    }
    
    private func resetRecord() {
        withAnimation {
            do {
                try modelContext.delete(model: HistoryItem.self)
                try modelContext.save()
            } catch {
                print("Failed to reset record: \(error)")
            }
        }
    }
}

#Preview {
    ContentView()
        .modelContainer(for: HistoryItem.self, inMemory: true)
}
