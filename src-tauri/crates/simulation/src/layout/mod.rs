
mod signal;
mod switch;
mod track;
mod track_circuit;
use std::collections::HashMap;
use track_circuit::TrackCircuit;
use track_circuit::TrackCircuitId;

use crate::layout::signal::Signal;
use crate::layout::signal::SignalId;
use crate::layout::switch::Switch;
use crate::layout::switch::SwitchId;
use crate::layout::track::TrackEdge;
use crate::layout::track::TrackEdgeId;
use crate::layout::track::TrackNode;
use crate::layout::track::TrackNodeId;

struct SimulationLayout {
    track_circuits: HashMap<TrackCircuitId, TrackCircuit>,
    signals: HashMap<SignalId, Signal>,
    track_nodes: HashMap<TrackNodeId, TrackNode>,
    track_edges: HashMap<TrackEdgeId, TrackEdge>,
    switches: HashMap<SwitchId, Switch>,
}

impl SimulationLayout {}
