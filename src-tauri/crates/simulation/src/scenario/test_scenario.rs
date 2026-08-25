use std::collections::HashMap;

use crate::{
    diagram::{DiagramPosition, DiagramSignal, DiagramSwitch, DiagramTrack, SignalDiagram},
    layout::{switch::SwitchId, test_layout::make_test_layout},
    scenario::Scenario,
};

pub fn make_test_scenario() -> Scenario {
    Scenario {
        id: String::from("T1"),
        name: String::from("Test 1"),
        description: String::from("This is a description"),
        layout: make_test_layout(),
        diagram: SignalDiagram {
            tracks: vec![DiagramTrack {
                positions: vec![
                    DiagramPosition { x: 150.0, y: 150.0 },
                    DiagramPosition {
                        x: 200.00,
                        y: 200.00,
                    },
                ],
                track_circuits: vec![String::from("TC1")],
            }],
            signals: vec![DiagramSignal {
                signal_id: String::from("S1"),
                position: DiagramPosition { x: 190.0, y: 170.0 },
            }],
            switches: HashMap::new(),
        },
    }
}
