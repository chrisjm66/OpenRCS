import { invoke } from "@tauri-apps/api/core";

export function getLayouts(): SimulationLayout[] {
    const layouts = invoke('get_layouts')
    console.log(layouts)
}

interface SimulationLayout {
    track_nodes: Map<TrackNodeId, TrackNode>,
    track_edges: Map<TrackEdgeId, TrackEdge>,
    switches: Map<SwitchId, Switch>,
    signals: Map<SignalId, Signal>,
    track_circuits: Map<TrackCircuitId, TrackCircuit>
}

type TrackNodeId = string
type TrackEdgeId = string
type SwitchId = string
type SignalId = string
type TrackCircuitId = string

interface TrackNode {
    position: Point
    track_type: TrackType
}

interface Point {
    x: number,
    y: number
}

enum TrackType {
    Boundary,
    Buffer,
    Switch,
    Crossing
}