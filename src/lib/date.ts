export function getFormattedDate() {
    return new Date().toLocaleString('en-GB', {
    timeZone: 'America/New_York',
    hourCycle: 'h23',
    hour: "2-digit",
    minute: "2-digit",
    second: "2-digit"
});
}