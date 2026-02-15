//
//  SidebarItem.swift
//  MyFirstMacOSApp
//
//  Created by neko3cs on 2026/02/14.
//

import Foundation

enum SidebarItem: String, CaseIterable, Identifiable {
    case counter = "カウンター"
    case history = "履歴"
    case httpCat = "HttpCat"
    case posts = "Posts"
    
    var id: String { self.rawValue }
    
    var icon: String {
        switch self {
        case .counter: return "number.circle"
        case .history: return "clock.arrow.circlepath"
        case .httpCat: return "cat.fill"
        case .posts: return "text.bubble.fill"
        }
    }
}
