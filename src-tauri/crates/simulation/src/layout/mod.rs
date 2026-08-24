pub mod signal;
pub mod switch;
pub mod test_layout;
pub mod track;
pub mod track_circuit;
use std::collections::HashMap;
use std::fs::File;
use specta::Type;
use std::io::BufReader;
use std::io::BufWriter;
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
use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize, Type)]
pub struct SimulationLayout {
    pub track_circuits: HashMap<TrackCircuitId, TrackCircuit>,
    pub signals: HashMap<SignalId, Signal>,
    pub track_nodes: HashMap<TrackNodeId, TrackNode>,
    pub track_edges: HashMap<TrackEdgeId, TrackEdge>,
    pub switches: HashMap<SwitchId, Switch>,
}

impl SimulationLayout {
    fn save_to_json(
        save_path: &str,
        simulation_layout: &SimulationLayout,
    ) -> Result<(), Box<dyn std::error::Error>> {
        let output = File::create(save_path)?;
        let writer = BufWriter::new(output);

        serde_json::to_writer_pretty(writer, &simulation_layout)?;
        Ok(())
    }

    fn load_layout(
        file_path: &str
    ) -> Result<SimulationLayout, Box<dyn std::error::Error>> {
       let file = File::open(file_path)?;
       let reader = BufReader::new(file);

       let layout = serde_json::from_reader(reader)?; 
       Ok(layout)
    }
}
