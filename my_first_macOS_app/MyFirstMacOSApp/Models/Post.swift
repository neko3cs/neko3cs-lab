//
//  Post.swift
//  MyFirstMacOSApp
//
//  Created by opencode on 2026/02/15.
//

import Foundation

struct Post: Identifiable, Codable, Sendable {
    let userId: Int
    let id: Int
    let title: String
    let body: String
}
