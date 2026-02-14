//
//  HistoryItem.swift
//  MyFirstMacOSApp
//
//  Created by neko3cs on 2026/02/14.
//

import Foundation
import SwiftData

@Model
final class HistoryItem {
    var timestamp: Date
    
    init(timestamp: Date) {
        self.timestamp = timestamp
    }
}
