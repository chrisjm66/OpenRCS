pub struct TrackNodeId(String);

pub struct TrackEdgeId(String);

pub struct TrackNode {
    position: Point,
    track_type: TrackType,
}

pub struct TrackEdge {
    from: TrackNodeId,
    to: TrackNodeId,
    geometry: Vec<Point>,
    properties: TrackProperties,
    allows_from_to: bool,
    allows_to_from: bool,
}

pub struct TrackProperties {
    electrified: bool,
    speed_limit: u32,
}

pub struct Point {
    x: f64,
    y: f64,
}

pub struct EdgeEnd {
    node_id: TrackNodeId,
    edge_id: TrackEdgeId,
}

pub enum TrackType {
    Boundary,
    Buffer,
    Switch,
    Crossing,
}
