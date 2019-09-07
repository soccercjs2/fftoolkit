BEGIN TRAN

--Delete All Draft Picks
DELETE FROM DraftPick

-- Undo Last Pick
DELETE FROM DraftPick
WHERE DraftPickId = (SELECT MAX(DraftPickId) FROM DraftPick)

--Set Draft Order
UPDATE DraftParticipant SET Name = 'Collin' WHERE DraftPosition = 1
UPDATE DraftParticipant SET Name = 'Brandon' WHERE DraftPosition = 2
UPDATE DraftParticipant SET Name = 'Paul' WHERE DraftPosition = 3
UPDATE DraftParticipant SET Name = 'Chris' WHERE DraftPosition = 4
UPDATE DraftParticipant SET Name = 'Blake' WHERE DraftPosition = 5
UPDATE DraftParticipant SET Name = 'Travis' WHERE DraftPosition = 6
UPDATE DraftParticipant SET Name = 'Dan' WHERE DraftPosition = 7
UPDATE DraftParticipant SET Name = 'Jordan' WHERE DraftPosition = 8
UPDATE DraftParticipant SET Name = 'Eric' WHERE DraftPosition = 9
UPDATE DraftParticipant SET Name = 'Nick' WHERE DraftPosition = 10
UPDATE DraftParticipant SET Name = 'Matt' WHERE DraftPosition = 11
UPDATE DraftParticipant SET Name = 'Tim' WHERE DraftPosition = 12

ROLLBACK TRAN

SELECT *
FROM DraftParticipant

--5	1	1	6	soccercjs2@gmail.com
--6	NULL	2	6	Brandon
--7	NULL	3	6	Paul
--8	NULL	4	6	Chris
--9	NULL	5	6	Blake
--10	NULL	6	6	Travis
--11	NULL	7	6	Dan
--12	NULL	8	6	Jordan
--13	NULL	9	6	Eric
--14	NULL	10	6	Nick
--16	3	11	6	matt.morgan@test.com
--17	4	12	6	tim.persson@test.com