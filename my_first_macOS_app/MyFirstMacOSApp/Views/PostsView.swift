//
//  PostsView.swift
//  MyFirstMacOSApp
//
//  Created by opencode on 2026/02/15.
//

import SwiftUI

struct PostsView: View {
    @State private var posts: [Post] = []
    @State private var isLoading = false
    @State private var errorMessage: String?
    
    var body: some View {
        VStack {
            if isLoading {
                ProgressView("読み込み中...")
            } else if let errorMessage = errorMessage {
                ContentUnavailableView("読み込み失敗", systemImage: "exclamationmark.triangle", description: Text(errorMessage))
                Button("再試行") {
                    fetchPosts()
                }
            } else {
                List(posts) { post in
                    VStack(alignment: .leading, spacing: 5) {
                        Text(post.title)
                            .font(.headline)
                        Text(post.body)
                            .font(.body)
                            .foregroundStyle(.secondary)
                            .lineLimit(2)
                    }
                    .padding(.vertical, 5)
                }
                .overlay {
                    if posts.isEmpty {
                        ContentUnavailableView("No Posts", systemImage: "text.bubble")
                    }
                }
            }
        }
        .navigationTitle("Posts")
        .onAppear {
            if posts.isEmpty {
                fetchPosts()
            }
        }
    }
    
    private func fetchPosts() {
        guard let url = URL(string: "https://jsonplaceholder.typicode.com/posts") else {
            errorMessage = "Invalid URL"
            return
        }
        
        isLoading = true
        errorMessage = nil
        
        URLSession.shared.dataTask(with: url) { data, response, error in
            DispatchQueue.main.async {
                isLoading = false
                
                if let error = error {
                    errorMessage = error.localizedDescription
                    return
                }
                
                guard let data = data else {
                    errorMessage = "No data received"
                    return
                }
                
                do {
                    posts = try JSONDecoder().decode([Post].self, from: data)
                } catch {
                    errorMessage = "Decoding failed: \(error.localizedDescription)"
                }
            }
        }.resume()
    }
}

#Preview {
    PostsView()
}
