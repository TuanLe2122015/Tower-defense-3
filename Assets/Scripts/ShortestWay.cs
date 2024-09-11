using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestWay : MonoBehaviour
{
    public const int rows = 39, columns = 12;
    bool[,] adjacencyMatrix = new bool[rows, columns];// Thể hiện của Tower
    bool[,] pointMatix = new bool[rows, columns];// Thể hiện của các nút
    float Y_axis = 2.5f;

    List<PointOfWeightMatrix> listPointMatrix = new List<PointOfWeightMatrix>();
    public const int maxPoint = 300;
    float[,] weightMatrix = new float[maxPoint, maxPoint];
    public const float const_H = 0.8f;
    class PointOfWeightMatrix
    {
        public int row, column;
        public PointOfWeightMatrix(int r, int c)
        {
            row = r;
            column = c;
        }
    }

    public GameObject StartPointOfEnemy;
    public GameObject EndPointOfEnemy;

    // Start is called before the first frame update
    public GameObject towerSpotRefab;
    public GameObject startSpotInstiate;
    public GameObject testPoint;

    public GameObject MatrixPoint;// Để lưu test Point khi Instantiate

    // For Dijkstra
    int[] arrayTrace = new int[maxPoint];
    float[] D = new float[maxPoint];
    bool[] P = new bool[maxPoint];
    int U = 0, S = 0, U_End, S_Start;
    int[] path_Temp = new int[maxPoint];
    bool path_True = false;
    public GameObject pathOfEnemy;
    public GameObject StartEndOfPathEnemy;
    int countPointOfBestWay;
    int countPointOfMatrix = 0;

    // Dùng ở hàm Awake, IndentifyPointOfBestWay
    int c_0, r_0, c_1, r_1;

    private void Awake()
    {
        // Khởi tạo TowerSpot trên toàn bản đồ
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 spotPosition = new Vector3(startSpotInstiate.transform.position.x + (float)(1.6 * j), startSpotInstiate.transform.position.y, startSpotInstiate.transform.position.z - (float)(1.6 * i));
                GameObject.Instantiate(towerSpotRefab, spotPosition, Quaternion.identity).transform.SetParent(transform, true);
                adjacencyMatrix[i, j] = true;// Khởi tạo ma trận của Tower, tại vị trí chưa có Tower thì là true
            }
        }
        //Ẩn các tower sopt khi mới vào game
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false); // Khi khởi đầu game ẩn các Tower Spot
        }

        // Mặc định trọng số là vô cực
        for (int i=0;i< maxPoint; i++)
        {
            for(int j=0; j< maxPoint; j++)
            {
                if (i == j)
                    weightMatrix[i, j] = 0;
                else
                    weightMatrix[i, j] = Mathf.Infinity;
                
            }
        }
        startSpotInstiate.SetActive(false);

        // chỉ kiểm tra, in ra
        //int countD = 0;
        //for (int r = 0; r < rows; r++)
        //    for (int c = 0; c < columns; c++)
        //    {
        //        if (adjacencyMatrix[r, c])
        //            countD++;
        //    }
        //Debug.Log("Có: " + countD + " đỉnh");
        //Debug.Log("Có: " + transform.childCount + " spot");


        //Thêm điểm đầu và điểm cuối của enemy vào ma trận POint
        
        float temp;
        // Cài đặt ma trận liền kề bằng bội số của 1.6, là khoảng cách giữa các TowerSpot
        temp = (float)((startSpotInstiate.transform.position.z - EndPointOfEnemy.transform.position.z) / 1.6);// Điểm cuối của Enemy
        r_0 = (int)temp;
        if ((temp - (float)r_0) >= 0.5f)
        {
            r_0++;
        }
        temp = (float)((EndPointOfEnemy.transform.position.x - startSpotInstiate.transform.position.x) / 1.6);
        c_0 = (int)temp;
        if ((temp - (float)c_0) >= 0.5f)
        {
            c_0++;
        }
        pointMatix[r_0, c_0] = true;// Đã thêm đỉnh   K Ế T  T H Ú C   ở hàm CreateWieghtMatrix, ở phía dưới
    }
    // hàm dưới được gọi trên đối tượng Spot prefabs
    public void adjustMatrixWhenBuyTower(Transform tranOfTowerSpot)// Khi mua Tower mới thì cập nhật lại ma trận adjacencyMatrix
    {
        int c, r;
        float temp;
        // Cài đặt ma trận liền kề bằng bội số của 1.6, là khoảng cách giữa các TowerSpot
        temp = (float)((startSpotInstiate.transform.position.z - tranOfTowerSpot.position.z) / 1.6);
        r = (int)temp;
        if ((temp - (float)r) >= 0.5f)
        {
            r++;
        }
        temp = (float)((tranOfTowerSpot.position.x - startSpotInstiate.transform.position.x) / 1.6);
        c = (int)temp;
        if ((temp - (float)c) >= 0.5f)
        {
            c++;
        }
        adjacencyMatrix[r, c] = false;

        // Chỉ để kiểm tra, không ảnh hưởng logic game
        //IdentifyPiontsMatrix();
    }
    // Hàm chỉ dùng để kiểm tra, không ảnh hưởng logic game
    public void PrintMatrix()
    {
        // chỉ kiểm tra, in ra
        int countD = 0;
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < columns; c++)
            {
                if (adjacencyMatrix[r, c])
                    countD++;
            }
        Debug.Log("Có: " + countD + " đỉnh");
        Debug.Log("Có: " + transform.childCount + " spot");
    }
    //Kiểm tra có Tower nằm chỗ giao nhau giữa Tower đang xét và point đang xét hay không ( Point nằm ở vị trí góc của Tower )
    bool CheckPointTower(int rPoint, int cPoint, int rTower, int cTower)// Hỗ trợ cho hàm Check4FacePoint
    {
        if (!adjacencyMatrix[rTower, cPoint] || !adjacencyMatrix[rPoint, cTower])//Bốn mặt Tower là có Tower
        {
            return false;
        }

        return true;
    }
    // Kiểm tra 4 mặt Point có Tower hay không, có thì = false
    bool Check4FacePoint(int rP, int cP, int rTower, int cTower)// Hỗ trợ cho hàm IdentifyPiontsMatrix
    {
        if (!adjacencyMatrix[rP, cP])
            return false;// điều kiện làm sai Point của Tower khác là False

        if (CheckPointTower(rP, cP, rTower,cTower))// Kiểm tra chỗ giao nhau giữa Point và Tower có Tower khác hay không
        {
            //kiểm tra 4 mặt của Point có kề 2 mặt trống hay không
            if (rP - 1 >= 0 && cP + 1 < columns)// Kiểm tra Tower nằm trong map hay không
            {
                if (adjacencyMatrix[rP - 1, cP] && adjacencyMatrix[rP, cP + 1])
                {
                    return true;
                }
            }
            if (rP + 1 < rows && cP + 1 < columns)// Kiểm tra tower nằm trong map hay không
            {
                if (adjacencyMatrix[rP + 1, cP] && adjacencyMatrix[rP, cP + 1])
                {
                    return true;
                }
            }
            if (rP - 1 >= 0 && cP - 1 >= 0)// Kiểm tra tower nằm trong map hay không
            {
                if (adjacencyMatrix[rP - 1, cP] && adjacencyMatrix[rP, cP - 1])
                {
                    return true;
                }
            }
            if (rP + 1 < rows && cP - 1 >= 0)// Kiểm tra tower nằm trong map hay không
            {
                if (adjacencyMatrix[rP + 1, cP] && adjacencyMatrix[rP, cP - 1])
                {
                    return true;
                }
            }
        }

        return false; // điều kiện làm sai Point của Tower khác là False
        // Kiểm tra có thể có 3 tower ảnh hưởng đến Point
        // +1 - 1 = +2 - 2
        // +1 + 1 = +2 0
        // - 1 - 1 = 0 - 2
    }
    // Được gọi khi Point Matrix được xác định là false, kiểm tra 3 tower có thể có, ảnh hưởng đến Point Matrix
    bool Check3TowerToPoint(int rP, int cP, int rT, int cT)// Dùng hỗ trợ cho hàm IdentifyPiontsMatrix
    {
        // Point ở trên bên trái
        if (rP < rT && cP < cT)
        {
            // Xét 3 trụ nếu có, tại vị trí ảnh hưởng Point
            //  dựa vào vị trí tower 0 , - 2 ,,, - 2 , -2 ,,,, -2, 0 ( rT, cT )
            // dựa vào vị trí Point -1 -1, -1 +1, +1 -1 ( rP, cP )
            if (rP-1>=0 && cP-1>=0 && !adjacencyMatrix[rP-1,cP-1])// Chỉ số không vượt ngoài mảng và có trụ (= false)
            {
                if (Check4FacePoint( rP, cP, rP - 1, cP - 1))
                    return true;// Kiểm tra với Tower thứ 1
            }
            // Chỉ số không vượt ngoài mảng và có trụ (= false)
            if (rP - 1 >= 0 && cP + 1 < columns && !adjacencyMatrix[rP - 1, cP + 1])
            {
               if (Check4FacePoint(rP, cP, rP - 1, cP + 1))
                        return true;// Kiểm tra với Tower thứ 2
            }
            
            if (rP + 1 < rows && cP - 1 >= 0 && !adjacencyMatrix[rP + 1, cP - 1])
            {
                return Check4FacePoint(rP, cP, rP + 1, cP - 1);// Kiểm tra với Tower thứ 3
            }
        }
        
        // Point ở trên bên phải
        if(rP<rT&&cP>cT)
        {
            //Debug.Log("**************************\nTrụ ở trên bên phải.....");
            // Xét 3 trụ nếu có, tại vị trí ảnh hưởng Point
            // -2 , 0 ,,, -2 , +2 ,,, 0 , +2 
            // dựa vào vị trí Point -1 -1, -1 +1, +1 +1 ( rP, cP )

            // Chỉ số không vượt ngoài mảng và có trụ (= false)
            if (Check4FacePoint(rP, cP, rP - 1, cP - 1) && rP - 1 >= 0 && cP - 1 >= 0 && !adjacencyMatrix[rP - 1, cP - 1])
                return true;// Kiểm tra với Tower thứ 1
                
            if (rP - 1 >= 0 && cP + 1 < columns && !adjacencyMatrix[rP - 1, cP + 1])
            {
                if (Check4FacePoint(rP, cP, rP - 1, cP + 1))
                    return true;// Kiểm tra với Tower thứ 2
            }
            
            if (rP + 1 < rows && cP + 1 < columns && !adjacencyMatrix[rP + 1, cP + 1])
            {
                return Check4FacePoint(rP, cP, rP + 1, cP + 1);// Kiểm tra với Tower thứ 3
            }
        }

        // Point ở dưới bên trái
        if (rP > rT && cP < cT)
        {
            // Xét 3 trụ nếu có, tại vị trí ảnh hưởng Point
            //  dựa vào vị trí tower 0 , - 2 ,,, - 2 , -2 ,,,, -2, 0 ( rT, cT )
            // dựa vào vị trí Point -1 -1, +1 - 1, +1 -1 ( rP, cP )
            if (rP - 1 >= 0 && cP - 1 >= 0 && !adjacencyMatrix[rP - 1, cP - 1])// Chỉ số không vượt ngoài mảng và có trụ (= false)
            {
                if (Check4FacePoint(rP, cP, rP - 1, cP - 1))
                    return true;// Kiểm tra với Tower thứ 1
            }
            // Chỉ số không vượt ngoài mảng và có trụ (= false)
            if (rP + 1 < rows && cP - 1 >=0 && !adjacencyMatrix[rP + 1, cP - 1])
            {
                if (Check4FacePoint(rP, cP, rP - 1, cP + 1))
                    return true;// Kiểm tra với Tower thứ 2
            }

            if (rP + 1 < rows && cP - 1 >= 0 && !adjacencyMatrix[rP + 1, cP - 1])
            {
                return Check4FacePoint(rP, cP, rP + 1, cP - 1);// Kiểm tra với Tower thứ 3
            }
        }

        return false;
    }
    // khởi tạo các nút, thay đổi pointMatrix
    bool temp_C;
    void IdentifyPiontsMatrix()// Dùng cho hàm IndentifyPointOfBestWay
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Xét từng Tower
                if (!adjacencyMatrix[i, j])
                {
                    //Kiểm tra bốn góc của Tower
                    if (i + 1 < rows && j - 1 >= 0)// Point nằm trong map hay không
                    {
                        pointMatix[i + 1, j - 1] = Check4FacePoint(i + 1, j - 1, i, j);//Point ở quanh tower i, j

                        if (!pointMatix[i + 1, j - 1])
                            pointMatix[i + 1, j - 1] = Check3TowerToPoint(i + 1, j - 1, i, j);
                    }

                    if (i + 1 < rows && j + 1 <columns)// Point nằm trong map hay không
                    {
                        temp_C = pointMatix[i + 1, j + 1];
                        pointMatix[i + 1, j + 1] = Check4FacePoint(i + 1, j + 1, i, j);
                        if (!pointMatix[i + 1, j + 1] && temp_C)
                            pointMatix[i + 1, j + 1] = Check3TowerToPoint(i + 1, j + 1, i, j);
                    }

                    if (i - 1 >=0 && j - 1 >= 0)// Point nằm trong map hay không
                    {
                        pointMatix[i - 1, j - 1] = Check4FacePoint(i - 1, j - 1, i, j);
                        if (!pointMatix[i - 1, j - 1])
                            pointMatix[i - 1, j - 1] = Check3TowerToPoint(i - 1, j - 1, i, j);
                    }

                    if (i - 1 >=0 && j + 1 <columns)// Point nằm trong map hay không
                    {
                        pointMatix[i - 1, j + 1] = Check4FacePoint(i - 1, j + 1, i, j);
                        if (!pointMatix[i - 1, j + 1])
                            pointMatix[i - 1, j + 1] = Check3TowerToPoint(i - 1, j + 1, i, j);
                    }
                }
                
            }
        }

        countPointOfMatrix = 0;
        listPointMatrix.Clear();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (pointMatix[i, j])
                {
                    // Đếm có bao nhiêu Point trên bản đồ
                    countPointOfMatrix++;
                    listPointMatrix.Add(new PointOfWeightMatrix(i, j));
                }
            }
        }

        // Kiểm tra xác định các nút của đường di, không ảnh hưởng logic game, chỉ tạo các khối cầu tại các nút
        //Vector3 posTestPoint;
        //for (int i = 0; i < MatrixPoint.transform.childCount; i++)
        //{
        //    Destroy(MatrixPoint.transform.GetChild(i).gameObject);
        //}
        //for (int i = 0; i < rows; i++)
        //{
        //    for (int j = 0; j < columns; j++)
        //    {
        //        if (pointMatix[i, j])
        //        {
        //            posTestPoint = new Vector3(startSpotInstiate.transform.position.x + (j) * 1.6f, Y_axis, startSpotInstiate.transform.position.z - (i) * 1.6f);
        //            Instantiate(testPoint, posTestPoint, Quaternion.identity).transform.SetParent(MatrixPoint.transform);
        //        }
        //    }
        //}
        // ket thuc kiem tra ************
    }
    
    // Khởi tạo ma trận trọng số, nếu không liền kề thì trọng số là max
    void CreateWieghtMatrix() // Dùng cho hàm IdentifyPointOfBestWay
    {
        //Đặt lại ma trận trọng số (Reset weight Matrix )
        for (int i = 0; i < maxPoint; i++)
        {
            for (int j = 0; j < maxPoint; j++)
            {
                weightMatrix[i, j] = Mathf.Infinity;
            }
            weightMatrix[i, i] = 0;
        }
        
        for (int xA = 0; xA < columns; xA++)// So sánh từng Point A với các Point B còn lại
        {
            for(int yA=0; yA < rows; yA++)
            {
                if (pointMatix[yA, xA])
                {
                    for(int xB= 0; xB < columns; xB++)// So sánh từng Point A với các Point B còn lại
                    {
                        for(int yB = 0; yB < rows; yB++)// So sánh từng Point A với các Point B còn lại
                        {
                            // Lập phương trình đường thẳng AB, tìm được a, b, c ( ax + by + c = 0 )
                            float a = yA - yB;
                            float b = xB - xA;
                            float c = xA * yB - xB * yA;

                            bool adjacencyAB = true;
                            if (pointMatix[yB, xB] && (xA != xB || yA != yB)) // Hai điểm A và B không trùng nhau
                            {
                                //Debug.Log("A va B khong trung nhau .....");
                                // Tìm những điểm P có khoảng cách <= H ( với P là vị trí tower, H là khoảng cách tự cho, , để cho enemy đi qua Tower có điểm nút, ở đây thử const_H = 0.8 ), với mỗi P ta:
                                //...... Lập phương trình đường thẳng P với AB là pháp tuyến, tìm được aP, bP, cP
                                //...... Tìm tọa độ giao điểm của 2 đường thẳng AB và P, được C, xét C có nằm trong đoạn AB hay không
                                //...... Nếu C nằm trong đoạn AB thì 2 điểm A và B không kề nhau ( weight = vô cực), thoát vòng lặp
                                for (int xP = 0; xP < columns; xP++)// xét Tower có nằm giữa AB, là điểm P cản đường AB
                                {
                                    for (int yP = 0; yP < rows; yP++)
                                    {
                                        if (!adjacencyMatrix[yP, xP])
                                        {
                                            //Tính khoảng cách từ điểm P ( là Tower) đến đt AB ta được H
                                            float H = Mathf.Abs(xP * a + yP * b + c) / Mathf.Sqrt(a * a + b * b);
                                            // Tìm những điểm P có H < const_H, là những điểm cản đường, A B ko liền kề
                                            float aP = 0f, bP = 0f, cP = 0f;
                                            float xC = 0f, yC = 0f;

                                            

                                            if (H < const_H)// const_H Là độ rộng của tower ( const_H càng nhỏ thì càng điểm A và B càng dễ liền kề )
                                            {
                                                // chỉ kiểm tra không ảnh hưởng logic game
                                                //Debug.Log("||||||||||||||||||||||||||||||||||||||||");
                                                //Debug.Log("co tower gan AB");
                                                //Debug.Log("A: (" + yA + ", " + xA + ") -- B: (" + yB + ", " + xB + ")");
                                                //Debug.Log("P: (" + yP + "; " + xP + ") -- H: " + H);
                                                

                                                aP = xB - xA;
                                                bP = yB - yA;
                                                cP = xP * (xA - xB) + yP * (yA - yB);

                                                // Tìm tọa độ giao điểm giữa đt AB và đt vuông góc đi qua Towers, được điểm C ( xC, yC )
                                                if (a == 0)
                                                {
                                                    xC = -cP / aP;
                                                    yC = -c / b;
                                                }
                                                else
                                                {
                                                    if (aP == 0)
                                                    {
                                                        xC = -c / a;
                                                        yC = -cP / bP;
                                                    }
                                                    else
                                                    {
                                                        // Khi các hệ số a, b khác 0 ta tìm với công thức khác
                                                        yC = (c / a - cP / aP) / (bP / aP - b / a);
                                                        xC = (-b * yC - c) / a;
                                                    }
                                                }

                                                //Nếu điểm C( xC, yC ) nằm ngoài đoạn AB thì 2 điểm A và B liền kề
                                                //Điểm C nằm trong đoạn AB, có nghĩa là Tower cản đường đi enemy
                                                // vậy 2 điểm A và B không liền kề

                                                if (
                                                        (xC > xA && xC < xB) ||
                                                        (xC < xA && xC > xB) ||
                                                        (yC > yA && yC < yB) ||
                                                        (yC < yA && yC > yB)
                                                   )
                                                {
                                                    // Điểm AB không liền kề, bị Tower chặn giữa, không cần xét các P khác nữa
                                                    //Debug.Log("Có Tower giữa 2 Point.....");
                                                    adjacencyAB = false;
                                                    break;
                                                }

                                                // Chỉ kiểm tra không anh hưởng logic game
                                                //Debug.Log("a: " + a + ", b: " + b + ", c: " + c + " -- aP: " + aP + ", bP: " + bP + ", cP: " + cP);
                                                //Debug.Log("C: (" + yC + ", " + xC + ")");
                                            }
                                            //else
                                            //{
                                            //    //P cách AB đủ xa
                                            //    adjacencyAB = true;
                                            //}
                                        }
                                        if (adjacencyAB == false)
                                            break;
                                    }
                                    if (adjacencyAB == false)// Điểm AB không liền kề, bị tower chặn giữa, không cần xét các P khác nữa
                                        break;
                                }


                                //Debug.Log("||||||||||||||||||||||||||||||||||||||||");
                                // Kiểm tra giữa 2 điểm AB có Point nào hay không
                                //if(adjacencyAB)
                                //{
                                //    for (int xP = 0; xP < columns; xP++)
                                //    {
                                //        for (int yP = 0; yP < rows; yP++)
                                //        {
                                //            if (pointMatix[yP, xP] )
                                //            {
                                //                if ((yP != yA || xP != xA) && (yP != yB || xP != xB))// Point không phải là 2 điểm AB
                                //                {
                                //                    float temp_P = a * yP + b * xP + c;// Thế tọa độ Point vào PT dt AB

                                //                    //Chỉ kiểm tra không ảnh hưởng logic game
                                //                    //if (temp_P > -0.05f && temp_P < 0.05f)
                                //                    //{
                                //                    //    Debug.Log("|||||||||||||||||||||||||||||||||||||");
                                //                    //    Debug.Log("A: (" + yA + ", " + xA + ") -- B: (" + yB + ", " + xB + ")");
                                //                    //    Debug.Log("Point: (" + yP + ", " + xP + ") -- temp_P: " + temp_P);
                                //                    //}
                                //                    //// ************

                                //                    if (temp_P > -0.05f && temp_P < 0.05f) // Point nằm trên đường thẳng AB
                                //                    {
                                //                        if ((xP < xA && xP > xB) || 
                                //                            (xP > xA && xP < xB) ||
                                //                            (yP < yA && yP > yB) || 
                                //                            (yP > yA && yP < yB) )  //Point nằm TRONG đoạn AB
                                //                        {
                                //                            adjacencyAB = false;
                                //                        }
                                //                    }
                                //                }
                                //            }
                                //            if (adjacencyAB == false)
                                //            {
                                //                break;
                                //            }
                                //        }
                                //        if (adjacencyAB == false)
                                //        {
                                //            break;
                                //        }
                                //    }
                                //}

                                // Nếu xét hết tất cả P, thỏa 2 điều kiện trên, thì 2 điểm A và B kề nhau, tính khoảng cách AB lưu vào ma trận trọng số
                                if (adjacencyAB)
                                {
                                    int index_1 = 0, index_2 = 0;
                                    for (int l = 0; l < listPointMatrix.Count; l++)
                                    {
                                        if (listPointMatrix[l].column == xA && listPointMatrix[l].row == yA)// Point ung voi ma tran trong so
                                        {
                                            index_1 = l;
                                        }
                                        if (listPointMatrix[l].column == xB && listPointMatrix[l].row == yB)
                                        {
                                            index_2 = l;
                                        }
                                    }
                                    weightMatrix[index_1, index_2] = Mathf.Sqrt((xA - xB) * (xA - xB) + (yA - yB) * (yA - yB));
                                    weightMatrix[index_2, index_1] = weightMatrix[index_1, index_2];
                                }


                            }


                        }
                    }
                }
            }
        }

        // kiểm tra ma trận trọng số, không ảnh hưởng logic game
        //Debug.Log("*************************************************");
        //Debug.Log("*************************************************");
        //Debug.Log(" Trọng số ma trận: ");
        //Debug.Log("Có: " + listPointMatrix.Count + " đỉnh");
        //for (int i = 0; i < listPointMatrix.Count; i++)
        //{
        //    for (int j = 0; j < listPointMatrix.Count; j++)
        //    {
        //        if (weightMatrix[i, j] < Mathf.Infinity)
        //            Debug.Log("D1: (" + listPointMatrix[i].row + ", " + listPointMatrix[i].column + ") - D2: (" + listPointMatrix[j].row + ", " + listPointMatrix[j].column + ") -- Weight L: " + weightMatrix[i, j] + " -- Weight J: " + weightMatrix[j, i]);
        //    }
        //}
        //*********Kết thúc kiểm tra
    }
    public void BestWayForEnemy(Transform trans_Enemy,ref List<Transform> path_Of_Enemy, ref bool enemyOutMap)
    {
        //Debug.Log("Enemy Position : (" +
        //        trans_Enemy.position.x + ", " +
        //        trans_Enemy.position.y + ", " +
        //        trans_Enemy.position.z + ")");
        // Đặt vị trí của enemy vào Point Matrix
        float temp;
        temp = (float)((startSpotInstiate.transform.position.z - trans_Enemy.position.z) / 1.6);// Điểm đầu của enemy
        r_1 = (int)temp;
        if ((temp - (float)r_1) >= 0.5f)
        {
            r_1++;
        }
        temp = (float)((trans_Enemy.position.x - startSpotInstiate.transform.position.x) / 1.6);
        c_1 = (int)temp;
        if ((temp - (float)c_1) >= 0.5f)
        {
            c_1++;
        }

        if (r_1 > rows || c_1 > columns)
        {
            enemyOutMap = true;
            return;
        }
            
        //Debug.Log(" r-1 -- c_1: (" + r_1 +", "+ c_1 +")");

        bool isPoint = pointMatix[r_1, c_1]; // Nếu Enemy trùng tại vị trí có Point sẵn

        // Xác định lại Point
        IdentifyPiontsMatrix();

        pointMatix[r_1, c_1] = true;// Đã thêm đỉnh     B Ắ T   Đ Ầ U      ở hàm CreateWieghtMatrix, ở phía dưới
        if(!isPoint)
            listPointMatrix.Add(new PointOfWeightMatrix(r_1, c_1));
        
        // Tính trọng số ma trận
        CreateWieghtMatrix();

        //Xác định đỉnh bắt đầu, kết thúc dùng cho Dijktra
        for (int i = 0; i < listPointMatrix.Count; i++)
        {
            if (listPointMatrix[i].row == r_0 && listPointMatrix[i].column == c_0)
            {
                U_End = i;// Đỉnh kết thúc
            }
            if (listPointMatrix[i].row == r_1 && listPointMatrix[i].column == c_1)
            {
                S_Start = i;// Đỉnh bắt đầu
            }
        }
        //Debug.Log("Đường đi ngắn nhất từ " + S_Start + " : " +
        //        " --- Row: " + listPointMatrix[S_Start].row +
        //        " --- Colum: " + listPointMatrix[S_Start].column);

        path_True = false;

        // Thank you, Mr. Dijkstra 
        Dijkstra();
        
        path_Of_Enemy.Clear();
        if (path_True)
        {
            //StartPointOfEnemy.transform.SetParent(pathOfEnemy.transform);

            for (int i = 0; i < countPointOfBestWay; i++)// Tạo đường đi cho enemy
            {
                //GameObject.Instantiate(new GameObject(), new Vector3(startSpotInstiate.transform.position.x + listPointMatrix[path_Temp[i]].column * 1.6f, Y_axis, startSpotInstiate.transform.position.z - listPointMatrix[path_Temp[i]].row * 1.6f), Quaternion.identity, pathOfEnemy.transform);
                
                Vector3 posNodeEnemy = new Vector3(
                        startSpotInstiate.transform.position.x + listPointMatrix[path_Temp[i]].column * 1.6f,
                        Y_axis,
                        startSpotInstiate.transform.position.z - listPointMatrix[path_Temp[i]].row * 1.6f);

                GameObject tempNode = new GameObject();
                tempNode.transform.SetPositionAndRotation(posNodeEnemy, Quaternion.identity);
                path_Of_Enemy.Add(tempNode.transform);
            }

            // Chỉ kiểm tra
            //for (int i = 0; i < countPointOfBestWay; i++)
            //{
            //    Debug.Log("Đỉnh: " + path_Temp[i] + 
            //        " --- Row: " + listPointMatrix[path_Temp[i]].row + 
            //        " --- Colum: " + listPointMatrix[path_Temp[i]].column);
            //}
        }
        else
        {
            //Debug.Log("Không có đường đi.........");
        }

        pointMatix[r_1, c_1] = isPoint;// Enemy đã đi qua vị trí này, trả về giá trị trước đó
        // Xác định lại Point
        //IdentifyPiontsMatrix();
    }
    void Dijkstra()// Dùng cho hàm IndentifyPointOfBestWay
    {
        //Khởi tạo
        for (int i = 0; i < maxPoint; i++)
        {
            arrayTrace[i] = -1;
            D[i] = Mathf.Infinity;
            P[i] = false;
        }

        D[S_Start] = 0; // Khởi tạo tại đỉnh S bắt đầu
        
        for (int i = 0; i < listPointMatrix.Count; i++)
        {
            float Max = Mathf.Infinity;
            int uBest = 0; // tìm đỉnh u chưa dùng, có khoảng cách nhỏ nhất
            for (int j = 0; j < listPointMatrix.Count; j++)
            {
                if (D[j] < Max && P[j] == false)
                {
                    uBest = j;
                    Max = D[j];
                }
            }

            // cải tiến các đường đi qua u
            int u = uBest;
            P[u] = true;
            for (int h = 0; h < maxPoint; h++)// các đỉnh h là các đỉnh liền kề với u và chưa xét ( có nghĩa P[u] = false ) 
            {
                if (weightMatrix[h, u] < Mathf.Infinity && h != u && !P[h])
                {
                    int v = h;
                    float w = weightMatrix[h, u]; // Lấy trọng số
                    if (D[v] > D[u] + w)
                    {
                        D[v] = D[u] + w;
                        arrayTrace[v] = u;
                    }
                }
            }
        }

        // Kiểm tra mảng trace
        //Debug.Log("**************************** \nMang trace: ");
        //for (int i = 0; i < listPointMatrix.Count; i++)
        //{
        //    Debug.Log(arrayTrace[i]);
        //}

        // Truy ngược mảng trace để được đường đi
        U = U_End; S = S_Start;
        countPointOfBestWay = 0;
        if (U != S && arrayTrace[U] == -1)
        {
            path_True = false; // không có đường đi
        }
        else
        {
            int i = 0;
            while (U != -1)
            { // truy vết ngược từ u về S
              //path.push_back(u);
                path_Temp[i] = U;
                U = arrayTrace[U];
                i++;
            }
            countPointOfBestWay = i - 1;
            // cần reverse vì đường đi lúc này là từ u về S
            int l = 0;
            i = countPointOfBestWay - 1;
            int k = i / 2;
            while (i > k)// Đảo ngược đường đi để có đường đi đúng
            {
                int temp = path_Temp[i];
                path_Temp[i] = path_Temp[l];
                path_Temp[l] = temp;
                i--;
                l++;
            }

            path_True = true;
        }
    }
}
