use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize)]
pub struct TrackCircuit {
    pub edges: Vec<TrackCircuitId>,
}

pub type TrackCircuitId = String;
