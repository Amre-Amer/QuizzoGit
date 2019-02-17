<?php
$servername = "localhost";
$username = "root";
$password = "mysql";
$database = "training";

echo '<head><meta http-equiv="refresh" content="15"></head>';
echo date("Y-m-d H:i:s")."<br>";
// Create connection
$conn = new mysqli($servername, $username, $password, $database);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
} 

echo $database . " database connected successfully";

echo "<p><a href=quiz.php>Home</a><p>";

echo "<h1>Results</h1>";
$sort = $_GET['sort'];
if (!$sort) $sort = "timestamp DESC LIMIT 20";
$sql = "SELECT * FROM results ORDER BY ".$sort;
echo $sql."<hr>";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
    // output data of each row
    echo '<font size="2" face="Arial" >';
    echo "<a href=?sort=test>Sort by Test</a><p>";
    echo "<a href=?sort=timestamp%20DESC>Sort by Time</a><p>";
    echo "<a href=?sort=timestamp%20DESC%20LIMIT%2020>Lastest</a><p>";
    echo "<table bgcolor=bbbbbb>";
    $file = "results.csv";
    $s = ",";
    $txt = "id".$s."firstname".$s."lastname".$s."email".$s."test".$s."duration".$s."timeOff".$s."cntOff".$s."timestamp";
    $txt = $txt."\r\n";
    $s = "</td><td bgcolor=ffffff>"; 
    echo "<tr><td>";
    echo "id".$s."firstname".$s."lastname".$s."email".$s."test".$s."duration".$s."timeOff".$s."cntOff".$s."timestamp".$s."duration".$s."timeOff".$s."cntOff";
    echo "</td></tr>";
    echo "<tr><td>";
    echo "</td></tr>";
    $h = 10;
    $cnt = 0;
    while($row = $result->fetch_assoc()) {
        $cnt++;
        if ($cnt % 2 == 0) {
            $bg = "ffffff";
        } else {
            $bg = "ddffff";
        }
        echo "<tr><td bgcolor=$bg height=20>";
        $s = "</td><td bgcolor=$bg>"; 
        $wMax = 300;
        $w = $row["duration"];
        $graphDuration = "<table border=0><tr><td bgcolor=00ff00 width=$w height=$h><td bgcolor=888888 width=".($wMax - $w)." height=$h></td></td></tr></table>";
        $w = $row["timeOff"];
        $wMax = 20;
        $c = "ff0000";
        if ($w == 0) $c = "888888";
        $graphTimeOff = "<table border=0><tr><td bgcolor=$c width=$w height=$h><td bgcolor=888888 width=".($wMax - $w)." height=$h></td></td></tr></table>";
        $w = $row["cntOff"];
        $wMax = 10;
        $c = "ff0000";
        if ($w == 0) $c = "888888";
        $graphCntOff = "<table border=0><tr><td bgcolor=$c width=$w height=$h><td bgcolor=888888 width=".($wMax - $w)." height=$h></td></td></tr></table>";
        echo $row["id"].$s.$row["firstname"].$s.$row["lastname"].$s.$row["email"].$s.$row["test"].$s.$row["duration"].$s.$row["timeOff"].$s.$row["cntOff"].$s.$row["timestamp"].$s.$graphDuration.$s.$graphTimeOff.$s.$graphCntOff;
        echo "</td></tr>";
        $s = ",";
        $txt = $txt.$row["id"].$s.$row["firstname"].$s.$row["lastname"].$s.$row["email"].$s.$row["test"].$s.$row["duration"].$s.$row["timeOff"].$s.$row["cntOff"].$s.$row["timestamp"];
        $txt = $txt."\r\n";
    }
    echo "</table><p>";
    file_put_contents($file, $txt);
    echo "<a href=results.csv>CSV download (spreadsheet)</a>";
} else {
    echo "0 results";
}

$conn->close();
?>