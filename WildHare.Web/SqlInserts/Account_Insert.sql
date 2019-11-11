
  --===========================================================
  -- Generating Insert Data for Account
  --===========================================================


INSERT [dbo].[Account] (
    [AccountId],[AccountName],[Created]
)
VALUES
    (1, N'Capital ', CAST('6/4/2019 9:00:00 PM' AS DateTime2)),
	(2, N'Acme Systems', CAST('11/29/2019 10:00:00 PM' AS DateTime2)),
	(3, N'Acme Ind', CAST('6/8/2019 9:00:00 PM' AS DateTime2)),
	(4, N'Nakatomi Ind', CAST('10/17/2018 8:00:00 PM' AS DateTime2)),
	(5, N'Spacely Inc', CAST('11/26/2020 2:00:00 PM' AS DateTime2)),
	(6, N'Compudata Inc', CAST('10/26/2019 6:00:00 AM' AS DateTime2)),
	(7, N'Next ', CAST('5/10/2021 7:00:00 AM' AS DateTime2)),
	(8, N'Stark Industries', CAST('6/3/2020 9:00:00 PM' AS DateTime2)),
	(9, N'Next Corp', CAST('5/17/2021 8:00:00 AM' AS DateTime2)),
	(10, N'Mega Corporation', CAST('10/2/2020 10:00:00 PM' AS DateTime2)),
	(11, N'Acme LLC', CAST('8/15/2018 7:00:00 AM' AS DateTime2)),
	(12, N'Parallax ', CAST('5/22/2019 12:00:00 PM' AS DateTime2)),
	(13, N'Virtucon ', CAST('4/30/2018 5:00:00 PM' AS DateTime2)),
	(14, N'Wayne Inc', CAST('1/26/2019 12:00:00 AM' AS DateTime2)),
	(15, N'Spacely ', CAST('1/21/2018 12:00:00 PM' AS DateTime2)),
	(16, N'Parallax Industries', CAST('4/3/2018 2:00:00 PM' AS DateTime2)),
	(17, N'Oscorp Industries', CAST('9/28/2020 12:00:00 PM' AS DateTime2)),
	(18, N'Globax ', CAST('7/24/2020 8:00:00 AM' AS DateTime2)),
	(19, N'Acme Co', CAST('9/20/2018 3:00:00 AM' AS DateTime2)),
	(20, N'Virtucon Inc', CAST('8/3/2018 12:00:00 AM' AS DateTime2))

GO
