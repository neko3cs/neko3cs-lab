//
//  MyFirstMacOSAppTests.swift
//  MyFirstMacOSAppTests
//
//  Created by neko3cs on 2026/02/14.
//

import Testing
import Foundation
@testable import MyFirstMacOSApp

struct MyFirstMacOSAppTests {

    @MainActor
    @Test func sidebarItemProperties() async throws {
        let counter = SidebarItem.counter
        let counterRaw = counter.rawValue
        let counterIcon = counter.icon
        #expect(counterRaw == "カウンター")
        #expect(counterIcon == "number.circle")
        
        let history = SidebarItem.history
        let historyRaw = history.rawValue
        let historyIcon = history.icon
        #expect(historyRaw == "履歴")
        #expect(historyIcon == "clock.arrow.circlepath")
        
        let httpCat = SidebarItem.httpCat
        let httpCatRaw = httpCat.rawValue
        let httpCatIcon = httpCat.icon
        #expect(httpCatRaw == "HttpCat")
        #expect(httpCatIcon == "cat.fill")
    }

    @Test func historyItemInitialization() async throws {
        let now = Date()
        let item = HistoryItem(timestamp: now)
        #expect(item.timestamp == now)
    }

}
