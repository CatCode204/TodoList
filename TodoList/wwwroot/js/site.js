// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const addItemContainer = document.getElementById("add-item-container")

function Back() {
    addItemContainer.classList.add("invisible")
}

function ShowAddItemContainer() {
    addItemContainer.classList.remove("invisible")
}

$(".todo-item-statusbox").on("change", function () {
    const $checkbox = $(this);
    const $parentDiv = $checkbox.parent();
    const itemId = $checkbox.attr('data-id');
    const isChecked = this.checked;

    console.log("Updating Item status:", itemId, "Status:", isChecked);

    fetch('/', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            id: itemId,
            status: isChecked
        })
    })
        .then(response => {
            if (!response.ok) {
                // Nếu server lỗi, đảo ngược lại trạng thái checkbox trên giao diện
                $checkbox.prop('checked', !isChecked);
                alert("Can't update state!");
            }
            else
            {
                location.reload();
                if (isChecked) {
                    $parentDiv.removeClass('bg-secondary');
                    $parentDiv.addClass('bg-primary');
                }
                else {
                    $parentDiv.removeClass('bg-primary');
                    $parentDiv.addClass('bg-secondary');
                }
                console.log("State updated");
            }
        })
        .catch(error => {
            $checkbox.prop('checked', !isChecked);
            console.error("Connection error:", error);
        });
});