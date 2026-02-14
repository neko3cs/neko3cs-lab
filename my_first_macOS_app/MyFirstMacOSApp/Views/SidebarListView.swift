//
//  SidebarListView.swift
//  MyFirstMacOSApp
//
//  Created by opencode on 2026/02/14.
//

import SwiftUI

struct SidebarListView: View {
    @Binding var selection: SidebarItem?
    
    var body: some View {
        List(SidebarItem.allCases, selection: $selection) { item in
            NavigationLink(value: item) {
                Label(item.rawValue, systemImage: item.icon)
            }
        }
    }
}
