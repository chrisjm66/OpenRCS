use serde::{Deserialize, Serialize};
use specta::Type;

#[derive(Serialize, Deserialize, Type)]
pub struct TrackCircuit {
    pub edges: Vec<TrackCircuitId>,
}

pub type TrackCircuitId = String;
