import { useState, useEffect } from 'react'
import { AgGridReact } from 'ag-grid-react'; 
import "ag-grid-community/styles/ag-grid.css"; 
import "ag-grid-community/styles/ag-theme-quartz.css";
import Select from "react-dropdown-select";
import './App.css'
function App() {
    const [initialized, setInitialized] = useState(false);
    const [form, setForm] = useState({});
    const [categories, setCategories] = useState();
    const [category, setCategory] = useState(undefined);
    const [basePrice, setBasePrice] = useState(undefined);

    const [rowData, setRowData] = useState([]);

    const [colDefs] = useState([
        { field: "basePrice", cellRenderer: valueFormatter, minWidth: 70, maxWidth: 150 },
        { field: "categoryName" },
        {
            headerName: "Fees ($)",
            children: [
                { field: "categoryBaseUsingFee", cellRenderer: valueFormatter },
                { field: "categorySpecialFee", cellRenderer: valueFormatter },
                { field: "associationFee" },
                { field: "storageFee" },
            ],
        },
        { field: "calculatedPrice", cellRenderer: valueFormatter, maxWidth: 150 },
    ]);

    useEffect(() => {
        initialize();
    }, []);

    useEffect(() => {
        if (basePrice !== undefined && category !== undefined) {
            renderModel();
        }
    }, [basePrice, category]);

    const results = form === undefined
        ? <p>Fill mandatory fields</p>
        :
        <div className="current-car">
            <form id="currentCar">
                <div>
                <h4>User Input</h4>
                    <label>Price : </label>
                    <input name="basePrice" value={form.basePrice} readOnly />
                    <label>Type : </label>
                    <input name="categoryName" value={form.categoryName} readOnly />
                </div>
                <div>
                    <h4>Calculated</h4>
                    <label>Base : </label>
                    <input name="categoryBaseUsingFee" value={form.categoryBaseUsingFee} readOnly />
                    <label>Special : </label>
                    <input name="categorySpecialFee" value={form.categorySpecialFee} readOnly />
                    <label>Association : </label>
                    <input name="associationFee" value={form.associationFee} readOnly />
                    <label>Storage : </label>
                    <input name="associationFee" value={form.storageFee} readOnly />
                </div>
                <div>
                    <h4>Total</h4>
                    <input name="calculatedPrice" value={form.calculatedPrice} readOnly />
                </div>
            </form>     
        </div>

    const contents = initialized === false
        ? <p><em>Loading...for more details.</em></p>
        :
        <div className="contents">
            <div>
                <label>
                    Base Price : 
                </label>
                <input
                    name="price"
                    min="0"
                    type="number"
                    onBlur={(e) => setBasePrice(e.target.value)}
                />
            </div>    
            <Select
                name="category"
                placeholder="Category"
                options={categories}
                labelField="name"
                valueField="key"
                onChange={(values) => setCategory(values[0])}
                />
        </div>

    return (
        <div>
            <div>
                <h1>Cars</h1>
                <div>
                    {contents}
                </div>
                <div>
                    {results}
                </div>
            </div>
            <h1>Calculated Cars</h1>
            <div
                className="table-results ag-theme-quartz" 
                style={{ height: 500 }}
            >
                <AgGridReact
                    rowData={rowData}
                    columnDefs={colDefs}
                />
            </div>
        </div>
    )

    async function renderModel() {
        let putData = {
            basePrice: basePrice,
            categoryKey: category.key,
        };

        const response = await fetch("cars", {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(putData),

            });

        const data = await response.json();

        setForm(data.value[0]);
        setRowData(data.value);
    }
    async function populateCategories() {
        const response = await fetch("categories");
        const data = await response.json();
        setCategories(data);
    }
    async function populateCars() {
        const response = await fetch("cars");
        const data = await response.json();
        setRowData(data.value);
    }
    async function initialize() {
        populateCars();
        populateCategories().then(setInitialized(true));
    }
    function valueFormatter(params) {
        return params.value.toFixed(2);
    }
}

export default App
