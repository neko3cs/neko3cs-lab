//
//  NumberDisplay.swift
//  MyFirstMacOSApp
//
//  Created by neko3cs on 2026/02/14.
//

import SwiftUI

struct NumberDisplay: View {
    let value: Int
    
    var body: some View {
        Text("\(value)")
            .font(.system(size: 80, weight: .bold, design: .rounded))
            .contentTransition(.numericText())
            .animation(.snappy, value: value)
    }
}
