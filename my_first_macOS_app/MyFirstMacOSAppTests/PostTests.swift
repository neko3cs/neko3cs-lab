//
//  PostTests.swift
//  MyFirstMacOSAppTests
//
//  Created by opencode on 2026/02/15.
//

import Testing
import Foundation
@testable import MyFirstMacOSApp

struct PostTests {

    @MainActor
    @Test func decodePost() async throws {
        let json = """
        {
            "userId": 1,
            "id": 1,
            "title": "sunt aut facere",
            "body": "quia et suscipit"
        }
        """.data(using: .utf8)!
        
        let post = try JSONDecoder().decode(Post.self, from: json)
        
        #expect(post.userId == 1)
        #expect(post.id == 1)
        #expect(post.title == "sunt aut facere")
        #expect(post.body == "quia et suscipit")
    }
}
