use serde::{Deserialize, Serialize};

pub type TrackNodeId = String;

#[derive(Serialize, Deserialize)]
pub struct TrackNode {
    pub position: Point,
    pub track_type: TrackType,
}

pub type TrackEdgeId = String;

#[derive(Serialize, Deserialize)]
pub struct TrackEdge {
    pub from: TrackNodeId,
    pub to: TrackNodeId,
    pub geometry: Vec<Point>,
    pub properties: TrackProperties,
    pub allows_from_to: bool,
    pub allows_to_from: bool,
}

#[derive(Serialize, Deserialize)]
pub struct TrackProperties {
    pub electrified: bool,
    pub speed_limit: u32,
}

#[derive(Serialize, Deserialize)]
pub struct Point {
    pub x: f64,
    pub y: f64,
}

#[derive(Serialize, Deserialize)]
pub struct EdgeEnd {
    pub node_id: TrackNodeId,
    pub edge_id: TrackEdgeId,
}

#[derive(Serialize, Deserialize)]
pub enum TrackType {
    Boundary,
    Buffer,
    Switch,
    Crossing,
}
