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

$table = "replays";

echo "<h1>Replays</h1>";
$sort = $_GET['sort'];
if (!$sort) $sort = "timestamp DESC LIMIT 20";
$sql = "SELECT * FROM $table ORDER BY ".$sort;
echo $sql."<hr>";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
    // output data of each row
    echo '<font size="2" face="Arial" >';
    echo "<a href=?sort=test>Sort by Test</a><p>";
    echo "<a href=?sort=timestamp%20DESC>Sort by Time</a><p>";
    echo "<a href=?sort=timestamp%20DESC%20LIMIT%2020>Lastest</a><p>";
    echo "<table bgcolor=bbbbbb>";
    $file = "replays.csv";
    $s = ",";
    $txtHeader = "id".$s."firstname".$s."lastname".$s."email".$s."test".$s."posx".$s."posy".$s."posz".$s."eulx".$s."euly".$s."eulz".$s."timestamp";
    $txt = $txtHeader."\r\n";
    echo "<tr><td>";
    $sComma = $s;
    $s = "</td><td bgcolor=ffffff>"; 
    echo str_replace($sComma, $s, $txtHeader);
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
        $sHtml = $s;
        $txtOut = $row["id"].$s.$row["firstname"].$s.$row["lastname"].$s.$row["email"].$s.$row["test"].$s.$row["posx"].$s.$row["posy"].$s.$row["posz"].$s.$row["eulx"].$s.$row["euly"].$s.$row["eulz"].$s.$row["timestamp"];
        echo $txtOut;
        echo "</td></tr>";
        $s = ",";
        $txt = $txt.str_replace($sHtml, $s, $txtOut);
        $txt = $txt."\r\n";
    }
    echo "</table><p>".$cnt;
//    echo mysql_num_rows($result)." rows<p>";
    if (file_exists($file)) unlink($file);
    file_put_contents($file, $txt);
    echo "<a href=$table.csv>CSV download (spreadsheet)</a>";
} else {
    echo "0 replays";
}

$conn->close();
?>