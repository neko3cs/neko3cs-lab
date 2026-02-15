//
//  HttpCatView.swift
//  MyFirstMacOSApp
//
//  Created by opencode on 2026/02/14.
//

import SwiftUI

struct HttpCatView: View {
    private let statusCodes = [
        100, 101, 102, 200, 201, 202, 204, 206, 207, 300, 301, 302, 303, 304, 305, 307,
        400, 401, 402, 403, 404, 405, 406, 408, 409, 410, 411, 412, 413, 414, 415, 416,
        417, 418, 421, 422, 423, 424, 425, 426, 429, 431, 444, 450, 451, 500, 501, 502,
        503, 504, 506, 507, 508, 509, 510, 511
    ]
    
    @State private var currentCode: Int = 200
    
    var body: some View {
        VStack(spacing: 30) {
            Button(action: showRandomCat) {
                Label("ランダムな猫を表示", systemImage: "dice.fill")
                    .font(.headline)
            }
            .buttonStyle(.borderedProminent)
            .controlSize(.large)
            
            AsyncImage(url: URL(string: "https://http.cat/\(currentCode).jpg")) { phase in
                switch phase {
                case .success(let image):
                    image
                        .resizable()
                        .aspectRatio(contentMode: .fit)
                        .clipShape(RoundedRectangle(cornerRadius: 16))
                        .shadow(radius: 10)
                case .failure:
                    errorView
                case .empty:
                    ProgressView()
                @unknown default:
                    EmptyView()
                }
            }
            .frame(maxWidth: 500, maxHeight: 400)
            .id(currentCode)
            
            Text("Status Code: \(currentCode)")
                .font(.title3.monospacedDigit())
                .foregroundStyle(.secondary)
            
            Spacer()
        }
        .navigationTitle("HttpCat")
        .padding()
        .onAppear(perform: showRandomCat)
    }
    
    private var errorView: some View {
        ContentUnavailableView("取得失敗", systemImage: "wifi.exclamationmark")
            .frame(width: 300, height: 200)
    }
    
    private func showRandomCat() {
        withAnimation {
            currentCode = statusCodes.randomElement() ?? 200
        }
    }
}

#Preview {
    HttpCatView()
}
