use std::collections::HashMap;

use crate::layout::{
    SimulationLayout,
    signal::{Signal, SignalId},
    switch::{Switch, SwitchId},
    track::{
        EdgeEnd, Point, TrackEdge, TrackEdgeId, TrackNode, TrackNodeId, TrackProperties,
        TrackType::{Boundary, Buffer},
    },
    track_circuit::{self, TrackCircuit, TrackCircuitId},
};

pub fn make_test_layout() -> SimulationLayout {
    let track_nodes = create_track_nodes();
    let signals = create_signals();
    let track_circuits = create_track_circuits();
    let switches = create_switches();
    let track_edges = create_track_edges();

    SimulationLayout {
        name: String::from("Test"),
        description: String::from("description"),
        track_circuits: track_circuits,
        signals: signals,
        track_nodes: track_nodes,
        track_edges: track_edges,
        switches: switches,
    }
}

fn create_track_edges() -> HashMap<TrackEdgeId, TrackEdge> {
    let track_edges: HashMap<TrackEdgeId, TrackEdge> = HashMap::from([
        (
            String::from("E1"),
            TrackEdge {
                from: String::from("N1"),
                to: String::from("N2"),
                geometry: vec![Point {
                    x: f64::from(150),
                    y: f64::from(150),
                }],
                properties: TrackProperties {
                    electrified: false,
                    speed_limit: 30,
                },
                allows_from_to: true,
                allows_to_from: true,
            },
        ),
        (
            String::from("E2"),
            TrackEdge {
                from: String::from("N1"),
                to: String::from("N2"),
                geometry: vec![Point {
                    x: f64::from(150),
                    y: f64::from(150),
                }],
                properties: TrackProperties {
                    electrified: false,
                    speed_limit: 30,
                },
                allows_from_to: true,
                allows_to_from: true,
            },
        ),
    ]);

    return track_edges;
}

fn create_switches() -> HashMap<SwitchId, Switch> {
    let switches: HashMap<SwitchId, Switch> = HashMap::from([(
        String::from("S1"),
        Switch {
            common: EdgeEnd {
                node_id: String::from("N1"),
                edge_id: String::from("E1"),
            },
            normal: EdgeEnd {
                node_id: String::from("N1"),
                edge_id: String::from("E2"),
            },
            reverse: EdgeEnd {
                node_id: String::from("N1"),
                edge_id: String::from("E4"),
            },
        },
    )]);

    return switches;
}

fn create_track_circuits() -> HashMap<TrackCircuitId, TrackCircuit> {
    let track_circuits: HashMap<TrackCircuitId, TrackCircuit> = HashMap::from([
        (
            String::from("TC1"),
            TrackCircuit {
                edges: vec![String::from("E1")],
            },
        ),
        (
            String::from("TC2"),
            TrackCircuit {
                edges: vec![String::from("E2"), String::from("E3")],
            },
        ),
    ]);

    return track_circuits;
}

fn create_signals() -> HashMap<SignalId, Signal> {
    let signals: HashMap<SignalId, Signal> = HashMap::from([(
        String::from("S1"),
        Signal {
            approach: EdgeEnd {
                edge_id: String::from("E1"),
                node_id: String::from("N1"),
            },
        },
    )]);

    return signals;
}

fn create_track_nodes() -> HashMap<TrackNodeId, TrackNode> {
    let track_nodes: HashMap<TrackNodeId, TrackNode> = HashMap::from([
        (
            String::from("N1"),
            TrackNode {
                position: Point {
                    x: f64::from(100),
                    y: f64::from(100),
                },
                track_type: Buffer,
            },
        ),
        (
            String::from("N2"),
            TrackNode {
                position: Point {
                    x: f64::from(200),
                    y: f64::from(200),
                },
                track_type: Boundary,
            },
        ),
        (
            String::from("N3"),
            TrackNode {
                position: Point {
                    x: f64::from(300),
                    y: f64::from(300),
                },
                track_type: Buffer,
            },
        ),
        (
            String::from("N4"),
            TrackNode {
                position: Point {
                    x: f64::from(400),
                    y: f64::from(400),
                },
                track_type: Buffer,
            },
        ),
    ]);

    return track_nodes;
}
