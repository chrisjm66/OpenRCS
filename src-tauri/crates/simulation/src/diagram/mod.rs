use std::collections::HashMap;

use serde::{Deserialize, Serialize};
use specta::Type;

use crate::layout::{signal::SignalId, switch::SwitchId, track_circuit::TrackCircuitId};

#[derive(Serialize, Deserialize, Type, Clone, Debug)]
pub struct SignalDiagram {
    pub tracks: Vec<DiagramTrack>,
    pub signals: Vec<DiagramSignal>,
    pub switches: HashMap<SwitchId, DiagramSwitch>,
}

#[derive(Serialize, Deserialize, Type, Clone, Debug)]
pub struct DiagramPosition {
    pub x: f64,
    pub y: f64,
}

#[derive(Serialize, Deserialize, Type, Clone, Debug)]
pub struct DiagramTrack {
    pub positions: Vec<DiagramPosition>,
    pub track_circuits: Vec<TrackCircuitId>,
}

#[derive(Serialize, Deserialize, Type, Clone, Debug)]
pub struct DiagramSignal {
    pub signal_id: SignalId,
    pub position: DiagramPosition,
}

#[derive(Serialize, Deserialize, Type, Clone, Debug)]
pub struct DiagramSwitch {
    pub switch_id: SwitchId,
    pub position: DiagramPosition,
}
